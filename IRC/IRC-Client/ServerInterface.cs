using IRC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class ServerInterface : Form
    {
        ArrayList users;
        Client cli;

        AlterEventRepeater evRepeater;        

        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVDelDelegate(User user);


        public ServerInterface(ArrayList users, Client cli)
        {
            this.users = users;
            this.cli = cli;
            
            InitializeComponent();

            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            this.cli.svProxy.alterEvent += new AlterDelegate(evRepeater.Repeater);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void DoAlterations(Operation op, User user)
        {
            LVAddDelegate lvAdd;
            LVDelDelegate delUser;

            switch (op)
            {
                case Operation.NewUser:
                    lvAdd = new LVAddDelegate(userListView.Items.Add);
                    ListViewItem lvItem = new ListViewItem(new string[] { user.nickname });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;
                case Operation.DelUser:
                    delUser = new LVDelDelegate(RemoveAItem);
                    BeginInvoke(delUser, new object[] { user });
                    break;
            }
        }

        private void RemoveAItem(User it)
        {
            foreach (ListViewItem lvI in userListView.Items)
                if (lvI.Text.Equals(it.nickname))
                {
                    userListView.Items.Remove(lvI);
                    break;
                }
        }


        private void ServerInterface_Load(object sender, EventArgs e)
        {
            foreach(User us in users)
                userListView.Items.Add(us.nickname);           
        }

        private void ClientWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            cli.svProxy.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);

            cli.logOut();
            this.users = null;
        }

        private void logOutButton_Click(object sender, EventArgs e)
        {
            cli.svProxy.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);

            cli.logOut();
            this.Visible = false;
            this.users = null;
        }
    }
}
