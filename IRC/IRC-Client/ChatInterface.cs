using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class ChatInterface : Form
    {
        private Client cli;
        Tabcontrol tabcontrol;

        public ChatInterface(Client cli)
        {
            this.cli = cli;
            tabcontrol = new Tabcontrol(cli);
            tabcontrol.Size = new Size(500, 300);
            tabcontrol.Dock = DockStyle.Fill;
            Controls.Add(tabcontrol);

            InitializeComponent();
        }

        private void ChatInterface_Load(object sender, EventArgs e)
        {

        }

        public void AddTab(string receiver, string user)
        {
            //MessageBox.Show("CENAS " + receiver);
            tabcontrol.AddNewTab(receiver, user);
        }

        internal void AddTab(string receiver, Intermediate.Message msg)
        {
            
            tabcontrol.AddNewTab(receiver, msg);
        }

        internal void closeTab(string peerName)
        {
            tabcontrol.RemoveTab(peerName);
        }
    }
}
