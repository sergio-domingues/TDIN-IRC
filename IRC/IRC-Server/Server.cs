using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Data.SQLite;

namespace IRC_Server

{
    class Server : IRC.IServer 
    {
        public int port { get; set; }

        SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = ../../database.db; Version=3;");


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

        public override string logIn(string nickname, string password)
        {
            string sql = "select * from User where username = '" + nickname + "' and password = '" + password + "';";
            Console.WriteLine(sql);
            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader success = command.ExecuteReader();
            

            if (success.Read())
            {
                m_dbConnection.Close();
                return "<Server - LOG IN SUCCESS> Username: " + nickname + " password: " + password;
            }
            else {
                m_dbConnection.Close();
                return "<Server - LOG IN FAILURE> Username: " + nickname + " password: " + password;
            }

            
        }

        public override string signUp(string username, string nickname, string password)
        {
            if (username == "" || nickname == "" || password == "") {
                return "<Server - SIGN UP FAIL: MISSING FIELD> Username: " + nickname + " password: " + password;
            }

            string sql_check = "select * from User where username = '" + nickname + "' and password = '" + password + "';";

            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(sql_check, m_dbConnection);
            SQLiteDataReader success = command.ExecuteReader();


            if (success.Read())
            {
                m_dbConnection.Close();
                return "<Server - SIGN UP FAIL: NICK ALREADY TAKEN> Username: " + nickname + " password: " + password;
            }
            else
            {
                string sql = "insert into User(username, password, name) VALUES('" + nickname + "' , '" + password + "' , '" + username + "' );";
                SQLiteCommand commandRegister = new SQLiteCommand(sql, m_dbConnection);
                commandRegister.ExecuteNonQuery();
                m_dbConnection.Close();
                return "<Server - SIGN UP SUCCESS> Username: " + nickname + " password: " + password + " realname:" + username;
            }

            
            
            
            
        }

        public override string logOut()
        {
            throw new NotImplementedException();
        }
    }


}
