using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Threading;

namespace RPCApplication
{
    public class Server
    {
        Socket mainServerSocket;
        public Server()
        {
            IPEndPoint serverPoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ip"]), Convert.ToInt32(ConfigurationManager.AppSettings["port"]));
            mainServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mainServerSocket.Bind(serverPoint);
            mainServerSocket.Listen(100);
        }
        public void Start()
        {
            Console.WriteLine("Server has been started...");
            while (true)
            {
                Socket acceptedSocket = mainServerSocket.Accept();
                ServerCommunication newCommunication = new ServerCommunication(acceptedSocket);
                Thread serverCommunicationThread = new Thread(newCommunication.Start);
                serverCommunicationThread.Start();
            }

        }
    }
}
