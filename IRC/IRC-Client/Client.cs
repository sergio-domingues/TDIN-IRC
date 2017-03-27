using IRC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace IRC_Client
{
    public class Client
    {
        int svPort;
        public IServer svProxy;
        public User myUser;
        public ArrayList usersList;


        public Client(int port)
        {
            svPort = port;
            setupConfig();
        }

        public void setupConfig()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = 0;

            // Create the channel.
            TcpChannel clientChannel = new TcpChannel(props, null, provider);

            // Register the channel.
            ChannelServices.RegisterChannel(clientChannel, false);

            // Create an instance of the remote object.
            svProxy = (IServer)Activator.GetObject(typeof(IServer),
                "tcp://localhost:" + svPort + "/Server");         
        }

        public bool logIn(string nickname, string password)
        {            
            Console.WriteLine("<LOG IN> The client is invoking the remote object.");

            //TODO access to this client peercomunication service address / port
            string address = "TODO:HARDCODED";
            int port = -1;
            bool loggedIn;

            loggedIn = svProxy.logIn(nickname, password, address, port);

            //TODO - HANDLE FORM ON LOGGIN ERROR
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

            //Console.WriteLine("Log in result: " + users.ToString());
            return true;
        }

        internal void removeUserFromList(User us)
        {
            usersList.Remove(us);
        }

        internal void addUserToList(User us)
        {
            usersList.Add(us);
        }

        public void signUp(string username, string nickname, string password)
        {  // Invoke a method on the remote object.
            Console.WriteLine("<CLI - SIGN UP> The client is invoking the remote object.");
            Console.WriteLine("CLI - SIGN UP result: " + svProxy.signUp(username, nickname, password));
        }
        
        public void logOut()
        {
            svProxy.logOut(myUser);
            usersList = null;
        }

    }
}
