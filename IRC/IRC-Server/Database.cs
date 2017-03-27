using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Server
{
    class Database
    {
        SQLiteConnection m_dbConnection { get; set; }

        public Database()
        {
            Console.WriteLine("Database created.");
            m_dbConnection = new SQLiteConnection("Data Source = ./database.db; Version=3;");
        }

        public bool logIn(string nickname, string password)
        {
            string sql = "select * from user where username = '" + nickname + "' and password = '" + password + "';";
            return executeQuery(sql);            
        }

        public bool signUp(string realname, string nickname, string password)
        {
            string queryCheck, queryAdd;

            queryCheck = "select * from User where username = '" + nickname + "' and password = '" + password + "';";
            queryAdd = "insert into User(username, password, name) VALUES('" + nickname + "' , '" + password + "' , '" + realname + "' );";

            if (executeQuery(queryCheck))
                return false;

            if (executeQuery(queryCheck))  //user already exists
                return false;
            else
                return executeQuery(queryAdd);            
        }

        private bool executeQuery(string query)
        {
            SQLiteDataReader success;
            m_dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            success = command.ExecuteReader();

            m_dbConnection.Close();
            return success.Read();
        }
    }
}
