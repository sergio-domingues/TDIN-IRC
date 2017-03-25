using System;

namespace IRC
    
{
    public class RemoteObject : MarshalByRefObject //: IServer
    {
        
        //se houveer problemas de comunicaçao adicionar container
        //implementar como singleton
        private int callCount = 0;

        public int GetCount()
        {
            Console.WriteLine("CENAS");
            Console.WriteLine(callCount);
            callCount++;
            return (callCount);
        }

       /* public override string logIn()
        {
            return "LOGGEDIN";
        }*/
    }
}
