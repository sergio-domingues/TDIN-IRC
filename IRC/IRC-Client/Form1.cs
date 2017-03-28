﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class Form1 : Form
    {
        private Client cli;
        Tabcontrol tabcontrol;

        

        public Form1(Client cli)
        {
            this.cli = cli;
            InitializeComponent();
            cli.testEvent += ReceiveMessage;
        }

        //login
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (usernameTextBox.Text != null && usernameTextBox.Text.Length > 0)
            {
                Int32.TryParse(usernameTextBox.Text, out cli.TEST);
            }
            changeForm();
            if (cli.TEST != -1) {
                cli.connectChat(cli.TEST);
            }
            cli.logIn(nicknameTextBox.Text, passwordTextBox.Text);
            
        }

        //signup
        private void button2_Click(object sender, EventArgs e)
        {
            cli.signUp(usernameTextBox.Text, nicknameTextBox.Text, passwordTextBox.Text);
            
        }

        public void ReceiveMessage(IRC_Client.Intermediate.Message msg)
        {
            
            string sender = msg.sender;
            if (!cli.connected.ContainsKey(sender))
            {
                cli.connectChat(Int32.Parse(sender), msg); //pode ser substituido com pedido ao servidor ou na altura que um se conecta, conectam os dois...
                
                //AddTab(sender);
            }
        }

        public void AddTab(string receiver) {
           
            tabcontrol.AddNewTab(receiver);
        }

        public void changeForm() {
            button1.Visible = false;
            button2.Visible = false;
            usernameTextBox.Visible = false;
            passwordTextBox.Visible = false;
            nicknameTextBox.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
        
            this.Size = new Size(460, 260);

            tabcontrol = new Tabcontrol(cli);
            tabcontrol.Dock = DockStyle.Fill;
            this.Controls.Add(tabcontrol);

        }

        internal void AddTab(string receiver, Intermediate.Message msg)
        {
            tabcontrol.AddNewTab(receiver, msg);
        }
    }
}
