using IRC;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using static IRC_Client.Intermediate;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace IRC_Client
{
    public class Client
    {
        public static Client instance;
        int svPort;
        public int cliPort;
        IServer svProxy;
        bool loggedIn = false;
        Form1 chat;

        public Dictionary<string, IClient> connected = new Dictionary<string, IClient>();

        public event MessageReceived testEvent;

        public int TEST = -1;

        public Client(int svPort, int cliPort)
        {
            this.svPort = svPort;
            this.cliPort = cliPort;
            
            setupConfig();
            instance = this;
            
        }

        public void setForm(Form1 form) {
            chat = form;
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

        public void setupConfig()
        {
            // Create the channel.
            TcpChannel clientChannel = new TcpChannel();

            // Register the channel.
            ChannelServices.RegisterChannel(clientChannel, false);
/*
            // Register as client for remote object.
            WellKnownClientTypeEntry remoteType = new WellKnownClientTypeEntry(
                typeof(IServer), "tcp://localhost:" + svPort + "/Server");
            RemotingConfiguration.RegisterWellKnownClientType(remoteType);  

            // Create a message sink.
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                clientChannel.CreateMessageSink(
                    "tcp://localhost:"+ svPort + "/Server", null,
                    out objectUri);
            Console.WriteLine("The URI of the message sink is {0}.",
                objectUri);
            if (messageSink != null)
            {
                Console.WriteLine("The type of the message sink is {0}.",
                    messageSink.GetType().ToString());
            } 
   */         
            //==================================================

            // Create an instance of the remote object.
            svProxy = (IServer)Activator.GetObject(typeof(IServer),
                "tcp://localhost:" + svPort + "/Server");
        }

        public void logIn(string nickname, string password)
        {  // Invoke a method on the remote object.
            Console.WriteLine("<LOG IN> The client is invoking the remote object.");
            Console.WriteLine("Log in result: " +  svProxy.logIn(nickname, password));

            //initialize remote client object

            /*// Create the channel.
            TcpChannel channel = new TcpChannel();

            // Register the channel.
            ChannelServices.RegisterChannel(channel, false);*/

            // Register as client for remote object.
            WellKnownClientTypeEntry remoteType = new WellKnownClientTypeEntry(
                typeof(IClient), "tcp://localhost:" + cliPort + "/ClientRemote");
            RemotingConfiguration.RegisterWellKnownClientType(remoteType);
            Console.WriteLine("Port " + cliPort);
            /*
            // Create a message sink.
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                channel.CreateMessageSink(
                    "tcp://localhost:" + cliPort + "/ClientRemote", null,
                    out objectUri);
            Console.WriteLine("The URI of the message sink is {0}.",
                objectUri);
            if (messageSink != null)
            {
                Console.WriteLine("The type of the message sink is {0}.",
                    messageSink.GetType().ToString());
            }

            // Create an instance of the remote object.
            cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                "tcp://localhost:" + cliPort + "/ClientRemote");
             */

            //cliProxy.ReceiveMessage("pasteis", "cenas", DateTime.Now);
           
            
            
            //falta a verificacao
            loggedIn = true;


            //chat.changeForm();
            /*
            if (TEST != -1)
            {
                connectChat(TEST);

                //cliProxy.ReceiveMessage("pasteis", "cenas", DateTime.Now);
            }
            */
            //chat.Show();
            

        }

        

        

        public void signUp(string username, string nickname, string password)
        {  // Invoke a method on the remote object.
            Console.WriteLine("<CLI - SIGN UP> The client is invoking the remote object.");
            Console.WriteLine("CLI - SIGN UP result: " + svProxy.signUp(username, nickname, password));
        }

        public void connectChat(int port) {
            Console.WriteLine("CONECTING");
            IClient cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                    "tcp://localhost:" + port + "/ClientRemote");
            connected.Add(port.ToString(), cliProxy);
            Console.WriteLine("CONECTED");
            chat.AddTab(port.ToString());
            //Console.WriteLine("DEVIA CRIAR UMA TAB");
        }

        public void connectChat(int port, Intermediate.Message msg) {
            IClient cliProxy = (IClient)Activator.GetObject(typeof(IClient),
                    "tcp://localhost:" + port + "/ClientRemote");
            connected.Add(port.ToString(), cliProxy);
            chat.AddTab(port.ToString(), msg);
        }

        public void sendMessage(string sender, string msg, string receiver)
        {
            Console.WriteLine("PASTEISSSSSSSSSS");
            IClient proxy = connected[receiver];
            proxy.ReceiveMessage(cliPort.ToString(), msg, DateTime.Now);
        }

        

    }
}
