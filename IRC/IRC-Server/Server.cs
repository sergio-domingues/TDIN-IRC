using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using IRC;

namespace IRC_Server

{
    class Server // : IServer
    {
        public int port { get; set; }
        
        public Server(int port)
        {
            this.port = port;
            SetupConfig();
        }

       /* public Server()
        {
        } */

        public void SetupConfig()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary props = new Hashtable();
            props["port"] = port;

            // Create the server channel.
            TcpChannel serverChannel = new TcpChannel(props, null, provider);

            
            // Register the server channel.
            ChannelServices.RegisterChannel(serverChannel, false);

            // Show the name of the channel.
            Console.WriteLine("The name of the channel is {0}.",
                serverChannel.ChannelName);

            RemotingConfiguration.RegisterWellKnownServiceType(
                     typeof(RemoteObject), "RemoteObject.rem",
                       WellKnownObjectMode.Singleton);
        }

    /*    public override string logIn()
        {
            return "loggedIN";
        }*/
    }


}
