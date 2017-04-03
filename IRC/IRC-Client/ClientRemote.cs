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
using System.Windows.Forms;
using static IRC_Client.Intermediate;

namespace IRC_Client
{
    public class ClientRemote : IClient
    {
        public string port { get; set; }

        public List<Intermediate.Message> messageList = new List<Intermediate.Message>();
        public ClientRemote(bool nothing)
        {
            port = Config();
        }

        public ClientRemote()
        {
        }

        public string Config()
        {
            BinaryClientFormatterSinkProvider clProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider svProvider = new BinaryServerFormatterSinkProvider();
            svProvider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary props = new Hashtable();
            props["name"] = "remotingClient";
            props["port"] = 0;

            // Create the channel.
            TcpChannel clientChannel = new TcpChannel(props, clProvider, svProvider);

            // Register the channel.
            //ChannelServices.RegisterChannel(clientChannel, false);

            //get port
            var channelData = (ChannelDataStore)clientChannel.ChannelData;
            string port = new Uri(channelData.ChannelUris[0]).Port.ToString();

            // Show the name of the channel.
            Console.WriteLine("The port of the channel is {0}.", port);

            RemotingConfiguration.RegisterWellKnownServiceType(
                     this.GetType(), "ClientRemote",
                       WellKnownObjectMode.Singleton);

            return port;
        }

        public override void ReceiveMessage(String user, String msg, DateTime time, string port1)
        {
            //MessageBox.Show(user + " " + port1);
            Client.instance.ReceiveMessage(new Intermediate.Message { sender = user, message = msg, port= port1, timestamp = time });
        }

        public override bool queryChat(string peerName)
        {
            string queryMsg;
            DialogResult dialogResult;

            queryMsg = peerName + " wants to start a chat with you. You want to connect to it?";
            dialogResult = MessageBox.Show(queryMsg, "Chat invitation", MessageBoxButtons.YesNo);

            return (dialogResult == DialogResult.Yes ? true : false);
        }

        public override string getUserName()
        {
            return Client.instance.myUser.nickname;
        }
    }
}
