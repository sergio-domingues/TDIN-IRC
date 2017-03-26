using System;
using System.Collections;
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
    public partial class ServerInterface : Form
    {
        List<string> nicks = new List<string>();

        public ServerInterface(List<string> nicknames)
        {
            nicks = nicknames;
            InitializeComponent();
        }

        private void ServerInterface_Load(object sender, EventArgs e)
        {
            foreach(string nick in nicks)
                userListView.Items.Add(nick);
        }
    }
}
