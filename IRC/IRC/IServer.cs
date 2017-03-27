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

        public class AlterEventRepeater : MarshalByRefObject
        {
            public event AlterDelegate alterEvent;

            public override object InitializeLifetimeService()
            {
                return null;
            }

            public void Repeater(Operation op, User user)
            {
                if (alterEvent != null)
                    alterEvent(op, user);
            }
        }

    }
}
