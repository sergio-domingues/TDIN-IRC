using System;
using System.Collections;
using System.Collections.Generic;

namespace IRC
{
    public abstract class IServer : MarshalByRefObject
    {
        public abstract List<string> logIn(string nickname, string password);

        public abstract string signUp(string username, string nickname, string password);

        public abstract string logOut();

    }
}
