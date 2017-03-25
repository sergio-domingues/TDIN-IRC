using IRC;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace IRC_Client
{
    class Client
    {
        int svPort;

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
                typeof(IServer), "tcp://localhost:" + svPort + "/Server.rem");
            RemotingConfiguration.RegisterWellKnownClientType(remoteType);  

            // Create a message sink.
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                clientChannel.CreateMessageSink(
                    "tcp://localhost:"+ svPort + "/Server.rem", null,
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
            IServer service = (IServer)Activator.GetObject(typeof(IServer),
                "tcp://localhost:" + svPort + "/Server.rem");

            // Invoke a method on the remote object.
            Console.WriteLine("The client is invoking the remote object.");
            Console.WriteLine("The remote object has been called {0} times." +
                service.logIn());
        }

    }
}
