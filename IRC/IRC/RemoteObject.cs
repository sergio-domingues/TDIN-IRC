using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    public class RemoteObject : MarshalByRefObject
    {
        //se houveer problemas de comunicaçao adicionar container
        //implementar como singleton
        private int callCount = 0;

        public int GetCount()
        {
            callCount++;
            return (callCount);
        }
    }
}
