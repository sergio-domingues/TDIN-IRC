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
