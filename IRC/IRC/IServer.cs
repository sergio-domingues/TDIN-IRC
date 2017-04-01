using System;
using System.Collections;
using System.Collections.Generic;

namespace IRC
{
    public abstract class IServer : MarshalByRefObject
    {

        public abstract event AlterDelegate alterEvent;
        public abstract bool logIn(string nickname, string password, string address, string port);

        public abstract bool signUp(string username, string nickname, string password);

        public abstract void logOut(User user);

        public abstract ArrayList getUsersList();     

    }
}
