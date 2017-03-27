using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections.Generic;
using IRC;
using System.Threading;

namespace IRC_Server

{
    class Server : IServer 
    {
        public int port { get; set; }

        
        public Hashtable table = new Hashtable();
        public ArrayList users = new ArrayList();
        
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

        private List<string> getUserList()
        {
            List<string> l = new List<string>();
            foreach (string user in table.Keys)
                l.Add(user);

            return l;
        }

        public override ArrayList logIn(string nickname, string password)
        {
            table.Add(nickname, password);
            Console.WriteLine("<Server - LOG IN> Username: " + nickname + " password: " + password);

            AddUser(new User(Guid.NewGuid(), nickname));

            return users;           
        }       

        public override string signUp(string username, string nickname, string password)
        {
            return "<Server - SIGN UP> Username: " + nickname + " password: " + password + " realname:" + username;
        }

        public override void logOut(User us)
        {
            table.Remove(us.nickname);
            DelUser(us);
        }

        //===========================Remote events
        
        public override event AlterDelegate alterEvent;

        public ArrayList GetList()
        {
            Console.WriteLine("GetList() called.");
            return users;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
               
     
        public void AddUser(User user)
        {
            users.Add(user);
            NotifyClients(Operation.NewUser, user);
        }
       
        public void DelUser(User user)
        {
            remFromUsers(user);
            NotifyClients(Operation.DelUser, user);
        }

        public void remFromUsers(User us)
        {
            foreach(User user in users)
            {
                if (user.nickname == us.nickname)
                {
                    users.Remove(user);
                    break;
                }
            }
        }
    
        void NotifyClients(Operation op, User user)
        {
           // alterEvent?.Invoke(op, user);

            if (alterEvent != null)
            {
                Delegate[] invkList = alterEvent.GetInvocationList();

                foreach (AlterDelegate handler in invkList)
                {
                    new Thread(() => {
                        try
                        {
                            handler(op, user);
                            Console.WriteLine("Invoking event handler");
                        }
                        catch (Exception)
                        {
                            alterEvent -= handler;
                            Console.WriteLine("Exception: Removed an event handler");
                        }
                    }).Start();
                }
            }
        }

        public override string requestChat(User user)
        {
            throw new NotImplementedException();
        }
    }


}
