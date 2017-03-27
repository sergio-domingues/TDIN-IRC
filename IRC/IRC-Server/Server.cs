using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Data.SQLite;
using IRC;
using System.Threading;

namespace IRC_Server

{
    class Server : IServer 
    {
        public int port { get; set; }
        public Database db { get; set; }
      
        public ArrayList users = new ArrayList();
        
       public Server(int port)
        {
            this.port = port;

            db = new Database();
            db.logIn("asdasd", "12321321");



            SetupConfig();
        }

        public Server()
        {
           // db = new Database();
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
                     this.GetType(), "Server",
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
        
        public override ArrayList getUsersList()
        {
            return users;
        }

        //TODO integration with db
        public override bool logIn(string nickname, string password, string address, int port)
        {            
            Console.WriteLine("<Server - LOG IN> Username: " + nickname + " addr: " + address + " port: " + port);                      

            //user already logged or user not found on db
            if (userLoggedIn(nickname) || !db.logIn(nickname, password) )
                return false;

            User newUser = new User(nickname, address, port);          
            AddUser(newUser);

            return true;           
        }
       
        public override bool signUp(string username, string nickname, string password)
        {            
            return db.signUp(username, nickname, password);
        }

        public override void logOut(User us)
        {
            Console.WriteLine("<SERVER LOGOUT> " + us.nickname);
            DelUser(us);
        }

        private bool userLoggedIn(string nickname)
        {
            foreach (User us in users)
            {
                if (us.nickname == nickname)
                    return true;
            }
            return false;
        }
        //===========================Remote events

        public override event AlterDelegate alterEvent;
     
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

       
    }


}
