﻿using IRC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class ServerInterface : Form
    {       
        Client cli;

        AlterEventRepeater evRepeater;        

        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVDelDelegate(User user);


        public ServerInterface(Client cli)
        {
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
                    cli.addUserToList(user);  //update usersList                   
                    BeginInvoke(lvAdd, new object[] { lvItem }); //change GUI
                    break;
                case Operation.DelUser:
                    delUser = new LVDelDelegate(RemoveAItem);
                    BeginInvoke(delUser, new object[] { user }); //change GUI
                    break;
            }
        }

        private void RemoveAItem(User it)
        {
            //remove da lista de utilizadores logados que o utilizador conhece
            cli.removeUserFromList(it);

            //remove da gui
            foreach (ListViewItem lvI in userListView.Items)
                if (lvI.Text.Equals(it.nickname))
                {
                    userListView.Items.Remove(lvI);
                    break;
                }
        }

        private void ServerInterface_Load(object sender, EventArgs e)
        {
            usernameLabel.Text = cli.myUser.nickname;

            foreach (User us in cli.usersList)
            {
                //não mostra o proprio utilizador na lista
                if(us.nickname != cli.myUser.nickname)
                    userListView.Items.Add(us.nickname);
            }         
        }

        private void ClientWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            logOutAux();
        }

        private void logOutButton_Click(object sender, EventArgs e)
        {
            logOutAux();
            this.Visible = false;
        }

        private void logOutAux()
        {
            cli.svProxy.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);

            cli.logOut();
        }

      
    }
}