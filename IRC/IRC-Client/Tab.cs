using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace IRC_Client
{
    public partial class Tab : UserControl
    {
        Client cli;
        string receiver, user;
        Tabcontrol tabs;

        public Tab(Client cli, string receiver, Tabcontrol tabs, string user)
        {
            this.cli = cli;
            this.receiver = receiver;
            this.user = user;
            this.tabs = tabs;
            InitializeComponent();
            cli.testEvent += ReceiveMessage;
        }

        public Tab(Client cli, string receiver, Tabcontrol tabs, Intermediate.Message first)
        {
            this.cli = cli;
            this.receiver = receiver;
            this.tabs = tabs;
            this.user = first.sender;
            InitializeComponent();
            cli.testEvent += ReceiveMessage;
            listBox1.Items.Add(first.message);
        }

        public void ReceiveMessage(Intermediate.Message msg)
        {
            
            if (msg.sender == receiver) {
                listBox1.Items.Add(msg.message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(receiver, cli.myUser.nickname);
            cli.sendMessage(textBox1.Text, receiver, cli.myUser.nickname);
            listBox1.Items.Add("You: " + textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabs.RemoveTab(receiver);
            cli.disconnectChat(receiver);
            this.Hide();
        }
    }
}
