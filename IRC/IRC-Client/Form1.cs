using System;

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
            cli.logIn(nicknameTextBox.Text, passwordTextBox.Text);
        }

        //signup
        private void button2_Click(object sender, EventArgs e)
        {
            cli.signUp(usernameTextBox.Text, nicknameTextBox.Text, passwordTextBox.Text);
        }


    }
}
