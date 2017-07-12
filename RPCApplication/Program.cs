using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length>0)
            {
                if (args[0] == "client")
                {
                    Client client = new Client();
                    client.Start();
                }
                else
                {
                    Server server = new Server();
                    server.Start();
                }
            }
            else
            {
                Console.WriteLine("This application runs -Client- argument. Please enter -client- arguments.");
                Console.ReadLine();
            }
        }
    }
}
