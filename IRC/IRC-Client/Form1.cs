using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class Form1 : Form
    {
        private Client cli;

        public Form1()
        {
            InitializeComponent();

            //todo add code here
        }

        public Form1(Client cli)
        {
            this.cli = cli;
            InitializeComponent();
        }

        //login
        private void button1_Click(object sender, EventArgs e)
        {
            bool loggedIn = cli.logIn(nicknameTextBox.Text, passwordTextBox.Text);

            if (loggedIn)
            {
                Visible = false;
                ServerInterface serverInterface = new ServerInterface(cli);
                serverInterface.ShowDialog();
                Visible = true;
            } 
            /* else {  //todo show label "log in error"
             *  
             * 
             * }   
             * */              
        }

        //signup 
        private void button2_Click(object sender, EventArgs e)  //todo mudar de acordo com o metodo de signup do cliente
        {
            string nick = nicknameTextBox.Text;

            cli.signUp(usernameTextBox.Text, nick, passwordTextBox.Text);
            Visible = false;

            //ServerInterface serverInterface = new ServerInterface(nick);
           // serverInterface.ShowDialog();
           // Visible = true;
        }


    }
}
