using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;

namespace IRC_Server

{
    class Server : IRC.IServer 
    {
        public int port { get; set; }
        
        public Server(int port)
        {
            this.port = port;
            SetupConfig();
        }

        public Server()
        {
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
                     new Server().GetType(), "Server",
                       WellKnownObjectMode.Singleton);

            // Parse the channel's URI.
            string[] urls = serverChannel.GetUrlsForUri("Server");
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
        
        //se houveer problemas de comunicaçao adicionar container
        //implementar como singleton
        private int callCount = 0;

        public int GetCount()
        {
            Console.WriteLine("Users logged:" + callCount);
            callCount++;
            return (callCount);
        }

        public override string logIn()
        {
            return "logged in";
        }
    }


}
