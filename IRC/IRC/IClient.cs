using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    public abstract class IClient: MarshalByRefObject
    {
        public abstract void ReceiveMessage(String user, String msg, DateTime time);

        public abstract bool queryChat(string peerName);
    }
}
