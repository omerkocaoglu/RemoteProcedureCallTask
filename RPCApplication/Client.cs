using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Reflection;
using System.Globalization;

namespace RPCApplication
{
    public class Client
    {
        byte[] data;
        int dataSize;
        Socket clientSocket;
        List<byte> allMethods = new List<byte>();
        string selectionMethodName = string.Empty;
        public Client()
        {
            IPEndPoint clientPoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ip"]), Convert.ToInt32(ConfigurationManager.AppSettings["port"]));
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(clientPoint);
        }
        public void Start()
        {
            Console.WriteLine("Connected to the server...");
            ReceiveMethodInformationsFromServer();
        }

        private void ReceiveMethodInformationsFromServer()
        {
            data = new byte[1024];
            dataSize = clientSocket.Receive(data, 0, data.Length, 0);
            if (dataSize > 0)
            {
                allMethods = data.Take(dataSize).ToList();
            }
            Unpackage unpackage = new Unpackage(allMethods);
            CreateMethodsWithServerInformations(unpackage.Start());
        }

        private void CreateMethodsWithServerInformations(List<List<byte>> allMethods)
        {
            //sunucudan gelen bilgilerle metot gövdesini yeniden oluşturma.
            List<ClientSideMethods> clientSideAllMethods = new List<ClientSideMethods>();
            ClientCreateMethodsProcess createMethodsProcess = new ClientCreateMethodsProcess();
            foreach (List<byte> item in allMethods)
            {
                clientSideAllMethods.Add(createMethodsProcess.Start(item));
            }


            //sunucudan alınan byte array metot bilgileriyle metotlar oluşturuldu. şimdi client ekranına basılacak.
            Console.WriteLine("---Server's available methods---");
            Console.WriteLine("\n");
            foreach (var method in clientSideAllMethods)
            {
                Console.WriteLine("Method Name: " + method.MethodName);
                Console.WriteLine("Return Type: " + method.MethodReturnType);
                Console.WriteLine("Parameters Count: " + method.MethodsParameters.Count.ToString());
                if (method.MethodsParameters.Count > 0)
                {
                    for (int i = 0; i < method.MethodsParameters.Count; i++)
                    {
                        Console.WriteLine(string.Format("{0}. Parameter (byte): ", i + 1));
                    }
                }
                Console.WriteLine("\n");
            }
            while (true)
            {
                Console.Write("Select a method (write method name): ");
                selectionMethodName = Console.ReadLine();
                selectionMethodName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(selectionMethodName);
                foreach (var item in clientSideAllMethods)
                {
                    item.MethodsParameterValues.Clear();
                }
                ClientSideMethods selectedMethod = clientSideAllMethods.Where(x => x.MethodName.Contains(selectionMethodName)).FirstOrDefault();
                if (selectedMethod.MethodsParameters.Count > 0)
                {
                    for (int i = 0; i < selectedMethod.MethodsParameters.Count; i++)
                    {
                        Console.Write(string.Format("{0}. Parameter ({1}): ", i + 1, selectedMethod.MethodsParameters[i]));
                        var paramater = Console.ReadLine();
                        selectedMethod.MethodsParameterValues.Add(paramater);
                    }
                }

                SendSelectionMethodToTheServer(selectedMethod);
            }
        }

        private void SendSelectionMethodToTheServer(ClientSideMethods selectedMethod)
        {
            List<byte> sendingArray = new List<byte>();
            if (!selectedMethod.MethodName.Contains('$'))
            {
                selectedMethod.MethodName = selectedMethod.MethodName + '$';
            }
            sendingArray.AddRange(ASCIIEncoding.Default.GetBytes(selectedMethod.MethodName));
            for (int i = 0; i < selectedMethod.MethodsParameters.Count; i++)
            {
                if ((Enums.ParameterType)selectedMethod.MethodsParameters[i] == Enums.ParameterType.String)
                {
                    byte[] stringVariable = ASCIIEncoding.Default.GetBytes(selectedMethod.MethodsParameterValues[i].ToString());
                    sendingArray.Add(Convert.ToByte(stringVariable.Length));
                    sendingArray.AddRange(stringVariable);
                }
                else
                {
                    byte valueLength = Convert.ToByte(selectedMethod.MethodsParameterValues[i]);
                    sendingArray.Add(1);
                    sendingArray.Add(valueLength);
                }
            }
            data = new byte[1024];
            data = sendingArray.ToArray();
            clientSocket.Send(data);
        }
    }
}
