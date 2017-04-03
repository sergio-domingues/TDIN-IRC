using IRC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Windows.Forms;
using static IRC_Client.Intermediate;

namespace IRC_Client
{
    public class Client
    {
        public static Client instance;

        public User myUser;
        public string myChatPort;
        public ClientRemote myRemObj;

        public ChatInterface chat;
        private readonly string address = "localhost";

        string svPort;
        public IServer svProxy;

        public ArrayList usersList;
        public Dictionary<string, IClient> connected = new Dictionary<string, IClient>();

        public event MessageReceived testEvent;
        //public int TEST = -1;

        public Client(string svPort)
        {
            this.svPort = svPort;

            setupConfig();
            myChatPort = setupMyRemObj();

            instance = this;
        }

        public void setupConfig()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = 0;

            // Create the channel.
            TcpChannel clientChannel = new TcpChannel(props, null, provider);

            // Register the channel.
            ChannelServices.RegisterChannel(clientChannel, false);

            //get port
            var channelData = (ChannelDataStore)clientChannel.ChannelData;
            string port = new Uri(channelData.ChannelUris[0]).Port.ToString();

            // Show the name of the channel.
            Console.WriteLine("The name of the channel is {0}.",
               port);

            // Create an instance of the remote object.
            svProxy = (IServer)Activator.GetObject(typeof(IServer),
                "tcp://localhost:" + svPort + "/Server");
        }

        public bool logIn(string nickname, string password)
        {
            Console.WriteLine("<LOG IN> The client is invoking the remote object.");

            bool loggedIn;

            loggedIn = svProxy.logIn(nickname, password, address, myChatPort);

            if (!loggedIn)
                return false;

            usersList = svProxy.getUsersList();

            foreach (User us in usersList)
            {
                if (us.nickname.Equals(nickname))
                {
                    myUser = us;
                    break;
                }
            }

            chat = new ChatInterface(this);

            return true;
        }

        public string setupMyRemObj()
        {
            myRemObj = new ClientRemote(false);
            return myRemObj.port;
        }

        public void registerPeerRemObj(string cliPort)
        {
            // Create an instance of the peer remote object.
            IClient cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                "tcp://localhost:" + cliPort + "/ClientRemote");

            //cliProxy.ReceiveMessage("pasteis", "cenas", DateTime.Now);
            //falta a verificacao
            //   loggedIn = true;

            /*if (TEST != -1)
            {
                connectChat(TEST);
                //cliProxy.ReceiveMessage("pasteis", "cenas", DateTime.Now);
             */
        }

        public bool signUp(string username, string nickname, string password)
        {  // Invoke a method on the remote object.
            Console.WriteLine("<CLI - SIGN UP> The client is invoking the remote object.");

            return svProxy.signUp(username, nickname, password);
        }

        public void logOut()
        {
            svProxy.logOut(myUser);
            usersList = null;
        }

        internal void removeUserFromList(User us)
        {
            usersList.Remove(us);
        }

        internal void addUserToList(User us)
        {
            usersList.Add(us);
        }

        public void queryForConnection(string peer)
        {
            
            string peerPort;

            peerPort = getUserPort(peer);
            

            if (peerPort == null)
            {
                Console.WriteLine("<Queryforconnection> ERROR! User not found in userslist");
                return;
            }

            IClient cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                    "tcp://localhost:" + peerPort + "/ClientRemote");

           

            
            connectChat(peerPort, peer);
            
            
        }

        private string getUserPort(string peerName)
        {

            foreach (User us in usersList)
            {
                if (us.nickname == peerName)
                    return us.port;
            }

            return null;
        }

        public void connectChat(string port, string user)
        {
            if (!connected.ContainsKey(port.ToString())) {
                IClient cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                    "tcp://localhost:" + port + "/ClientRemote");
                connected.Add(port, cliProxy);
                //MessageBox.Show(port);
                chat.AddTab(port, user);
            }  
        }

        public void connectChat(int port, Intermediate.Message msg)
        {
            if (!connected.ContainsKey(port.ToString())) {
                IClient cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                   "tcp://localhost:" + port + "/ClientRemote");
                connected.Add(port.ToString(), cliProxy);
                chat.AddTab(port.ToString(), msg );
            }
           
        }

        public void disconnectChat(string port) {
            if (connected.ContainsKey(port)) {
                IClient cliProxy = connected[port];
                connected.Remove(port);
                
                closeTab(port);
            }
        }

        public void ReceiveMessage(Intermediate.Message msg)
        {
            MessageReceived listener = null;
            Delegate[] dels = testEvent.GetInvocationList();

            foreach (Delegate del in dels)
            {
                try
                {
                    listener = (MessageReceived)del;
                    listener.Invoke(msg);
                }
                catch (Exception ex)
                {
                    //Could not reach the destination, so remove it
                    //from the list
                    testEvent -= listener;
                }
            }
        }

        public void sendMessage(string msg, string receiver, string user)
        {
            IClient proxy = connected[receiver];
            proxy.ReceiveMessage(myChatPort, msg, DateTime.Now, user);
        }

        public void setForm(ChatInterface form)
        {
            chat = form;
        }

        public void closeTab(string peerName) {
            chat.closeTab(peerName);
        }


    }
}
