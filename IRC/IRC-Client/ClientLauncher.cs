using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRC_Client
{
    static class ClientLauncher
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Client cli = new Client("9000");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new Homepage(cli));
        }

        public static int GetFreeTcpPort()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
            int port = ((IPEndPoint)sock.LocalEndPoint).Port;
            sock.Close();

            return port;
        }
    }
}
