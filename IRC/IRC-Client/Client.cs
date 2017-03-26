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

        public Client(int port)
        {
            svPort = port;
            setupConfig();
        }

        public void setupConfig()
        {
            // Create the channel.
            TcpChannel clientChannel = new TcpChannel();

            // Register the channel.
            ChannelServices.RegisterChannel(clientChannel, false);

            // Register as client for remote object.
            WellKnownClientTypeEntry remoteType = new WellKnownClientTypeEntry(
                typeof(IServer), "tcp://localhost:" + svPort + "/Server");
            RemotingConfiguration.RegisterWellKnownClientType(remoteType);

            // Create a message sink.
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                clientChannel.CreateMessageSink(
                    "tcp://localhost:" + svPort + "/Server", null,
                    out objectUri);
            Console.WriteLine("The URI of the message sink is {0}.",
                objectUri);
            if (messageSink != null)
            {
                Console.WriteLine("The type of the message sink is {0}.",
                    messageSink.GetType().ToString());
            }

            //==================================================

            // Create an instance of the remote object.
            svProxy = (IServer)Activator.GetObject(typeof(IServer),
                "tcp://localhost:" + svPort + "/Server");
        }



        public ArrayList logIn(string nickname, string password)
        {  // Invoke a method on the remote object.
            ArrayList users;

            Console.WriteLine("<LOG IN> The client is invoking the remote object.");

            users = svProxy.logIn(nickname, password);
           
            foreach (User us in users)
            {
                if (us.nickname.Equals(nickname))
                {
                    myUser = us;
                    break;
                }
            }

            Console.WriteLine("Log in result: " + users.ToString());
            return users;
        }

        public void signUp(string username, string nickname, string password)
        {  // Invoke a method on the remote object.
            Console.WriteLine("<CLI - SIGN UP> The client is invoking the remote object.");
            Console.WriteLine("CLI - SIGN UP result: " + svProxy.signUp(username, nickname, password));
        }


        public void logOut()
        {
            svProxy.logOut(myUser);
        }

        //========================  




    }
}
