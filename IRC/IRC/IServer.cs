using System;

namespace IRC
{
    public abstract class IServer : MarshalByRefObject
    {
        public abstract string logIn(string nickname, string password);

        public abstract string signUp(string username, string nickname, string password);

        public abstract string logOut();

    }
}
