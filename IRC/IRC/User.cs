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
        public Guid guid { get; set; }
        public string nickname { get; set; }

        public User(Guid guid, string nick)
        {
            this.guid = guid;
            nickname = nick;
        }

        public User(string nick)
        {
            this.guid = Guid.NewGuid();
            nickname = nick;
        }
    }



}
