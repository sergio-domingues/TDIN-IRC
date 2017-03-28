using IRC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using static IRC_Client.Intermediate;

namespace IRC_Client
{
    class ClientRemote : IClient
    {


        public int port { get; set; }

        public ClientRemote(int port)
        {
            this.port = port;
            Console.WriteLine("REMOTE PORT " + port);
            Config();
        }

        public ClientRemote() { }

        public void Config()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary props = new Hashtable();
            props["port"] = port;
            Console.WriteLine("REMOTE PORT " + port);

            // Create the server channel.
            TcpChannel serverChannel = new TcpChannel(props, null, provider);


            // Register the server channel.
            //ChannelServices.RegisterChannel(serverChannel, false);

            // Show the name of the channel.
            Console.WriteLine("The name of the channel is {0}.",
                serverChannel.ChannelName);

            RemotingConfiguration.RegisterWellKnownServiceType(
                     new ClientRemote().GetType(), "ClientRemote",
                       WellKnownObjectMode.Singleton);

            // Parse the channel's URI.
            string[] urls = serverChannel.GetUrlsForUri("ClientRemote");
            if (urls.Length > 0)
            {
                string objectUrl = urls[0];
                string objectUri;
                string channelUri = serverChannel.Parse(objectUrl, out objectUri);
                Console.WriteLine("The object URL is {0}.", objectUrl);
                Console.WriteLine("The object URI is {0}.", objectUri);
                Console.WriteLine("The channel URI is {0}.", channelUri);
            }

        }


        
        public event MessageReceived MessageArrived;

        public List<Message> messageList = new List<Message>();

        public override void ReceiveMessage(String user, String msg, DateTime time)
        {
            
            Client.instance.ReceiveMessage(new Message { sender = user, message = msg, timestamp = time });
        }

    }
}
