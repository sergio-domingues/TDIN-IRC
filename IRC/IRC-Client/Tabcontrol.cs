using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IRC;

namespace IRC_Client
{
    public partial class Tabcontrol : UserControl
    {
        Client cli;

        public Tabcontrol(Client cli)
        {
            this.cli = cli;
            InitializeComponent();
        }

        public void AddNewTab(string receiver)
        {
            
            Tab myUserControl = new Tab(cli, receiver);
            myUserControl.Dock = DockStyle.Fill;
            TabPage myTabPage = new TabPage();//Create new tabpage
            myTabPage.Controls.Add(myUserControl);
            //tabControl1.TabPages.Add(myTabPage);
            tabControl1.Invoke(new Action(() => tabControl1.TabPages.Add(myTabPage)));
            
        }

        internal void AddNewTab(string receiver, Intermediate.Message msg)
        {
            Tab myUserControl = new Tab(cli, receiver, msg);
            myUserControl.Dock = DockStyle.Fill;
            TabPage myTabPage = new TabPage();//Create new tabpage
            myTabPage.Controls.Add(myUserControl);
            //tabControl1.TabPages.Add(myTabPage);
            tabControl1.Invoke(new Action(() => tabControl1.TabPages.Add(myTabPage)));
        }
    }
}
