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
        string receiver;

        public Tab(Client cli, string receiver)
        {
            this.cli = cli;
            this.receiver = receiver;
            InitializeComponent();
            cli.testEvent += ReceiveMessage;
        }

        public Tab(Client cli, string receiver, Intermediate.Message first)
        {
            this.cli = cli;
            this.receiver = receiver;
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
            cli.sendMessage(textBox1.Text, receiver);
            listBox1.Items.Add("You: " + textBox1.Text);
        }
    }
}
