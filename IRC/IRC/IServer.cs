using System;
using System.Collections;
using System.Collections.Generic;

namespace IRC
{
    public abstract class IServer : MarshalByRefObject
    {

        public abstract event AlterDelegate alterEvent;
        public abstract ArrayList logIn(string nickname, string password);

        public abstract string signUp(string username, string nickname, string password);

        public abstract void logOut(User us);

        /*
         * return receiver remote object address for communication          
         */
        public abstract string requestChat(User user);

    }
}
