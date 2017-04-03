using IRC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Client
{
    public class Intermediate: MarshalByRefObject {
        public delegate void MessageReceived(Message msg);

        [Serializable]
        public class Message
        {
            public String sender;
            public string port;
            public String message;
            public DateTime timestamp;
        }

        
    }

    
    

    

    



}
