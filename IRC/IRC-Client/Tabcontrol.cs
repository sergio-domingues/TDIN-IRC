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
        Dictionary<string, TabPage> tabList = new Dictionary<string, TabPage>();

       

        public Tabcontrol(Client cli)
        {
            this.cli = cli;
            InitializeComponent();
        }

        public void AddNewTab(string receiver, string user)
        {
            
            Tab myUserControl = new Tab(cli, receiver, this, user);
            myUserControl.Dock = DockStyle.Fill;
            TabPage myTabPage = new TabPage(user);//Create new tabpage
            myTabPage.Controls.Add(myUserControl);
            tabList.Add(receiver, myTabPage);
            //tabControl1.TabPages.Add(myTabPage);
            tabControl1.Invoke(new Action(() => tabControl1.TabPages.Add(myTabPage)));
            
        }

        internal void AddNewTab(string receiver, Intermediate.Message msg)
        {
            
            Tab myUserControl = new Tab(cli, receiver, this, msg);
            myUserControl.Dock = DockStyle.Fill;
            TabPage myTabPage = new TabPage(msg.port);//Create new tabpage
            myTabPage.Controls.Add(myUserControl);
            tabList.Add(receiver, myTabPage);
            //tabControl1.TabPages.Add(myTabPage);
            tabControl1.Invoke(new Action(() => tabControl1.TabPages.Add(myTabPage)));
            
        }

        public void RemoveTab(string receiver) {
            
            TabPage toRemove;
            if (tabList.TryGetValue(receiver, out toRemove)) {
                tabList.Remove(receiver);
                tabControl1.Invoke(new Action(() => tabControl1.TabPages.Remove(toRemove)));
            }
            
        }

       


    }
}
