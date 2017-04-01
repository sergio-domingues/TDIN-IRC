using System;

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class Homepage : Form
    {
        private Client cli;     

        public Homepage(Client cli)
        {
            this.cli = cli;

            Size = new Size(460, 260);

            InitializeComponent();
            cli.testEvent += ReceiveMessage;
        }

        //login
        private void button1_Click(object sender, EventArgs e)
        {
            bool loggedIn;                     

            loggedIn = cli.logIn(nicknameTextBox.Text, passwordTextBox.Text);

            if (loggedIn)
            {
                Hide();
                ServerInterface serverInterface = new ServerInterface(cli, this);
                serverInterface.Show();
                cli.chat.Show();                
                //Visible = true;
            }
            else
            {
                MessageBox.Show("Error! User already logged in or user does not exists on database.", "Log in",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }             
        }

        //signup 
        private void button2_Click(object sender, EventArgs e)  
        {
            bool result;

            result = cli.signUp(usernameTextBox.Text, nicknameTextBox.Text, passwordTextBox.Text);

            MessageBox.Show(result == true ? "Success" : "Error", "Sign up",
                MessageBoxButtons.OK,
                result == true ? MessageBoxIcon.Information : MessageBoxIcon.Error);            
        }
        
        public void ReceiveMessage(Intermediate.Message msg)
        {            
            string sender = msg.sender;
            if (!cli.connected.ContainsKey(sender))
            {
                cli.connectChat(Int32.Parse(sender), msg); //pode ser substituido com pedido ao servidor ou na altura que um se conecta, conectam os dois...
                
                //AddTab(sender);
            }
        }
              
    }
}
