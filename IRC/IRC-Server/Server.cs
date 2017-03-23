using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;
using System.Collections;

namespace IRC
{
    class Server
    {
        public int port { get; set; }
        
        public Server(int port)
        {
            this.port = port;
            SetupConfig();
        }

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

    }


}
