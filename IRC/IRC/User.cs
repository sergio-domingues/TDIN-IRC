using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    [Serializable]
    public class User
    {
        public string nickname { get; set; }
        public string address { get; set; }
        public int port { get; set; }

        public User(string nick, string address, int port)
        {
            nickname = nick;
            this.address = address;
            this.port = port;
        }
    
    }



}
