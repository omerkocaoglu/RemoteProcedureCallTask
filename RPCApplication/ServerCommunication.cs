using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class ServerCommunication
    {
        int dataSize;
        byte[] data;
        Socket acceptedSocket;
        public ServerCommunication(Socket acceptedSocket)
        {
            this.acceptedSocket = acceptedSocket;
        }
        public void Start()
        {
            List<List<byte>> allServerMethods = new List<List<byte>>();
            Type serverMethodsType = typeof(ServerMethods);
            MethodInfo[] serverMethods = serverMethodsType.GetMethods();
            MethodInfo[] serverRPCMethods = serverMethods.Where(x => x.GetCustomAttributes(typeof(RPCAttribute), true).Length > 0).ToArray();
            Package package = new Package();
            foreach (var rpcMethod in serverRPCMethods)
            {
                allServerMethods.Add(package.CompoundMethods(rpcMethod));
            }

            SendMethodsInformationToClient(allServerMethods);
            while (true)
            {
                data = new byte[1024];
                dataSize = acceptedSocket.Receive(data, 0, data.Length, 0);
                ReceiveSelectedMethodFromClient(data);
            }
        }

        private void ReceiveSelectedMethodFromClient(byte[] methodArray)
        {
            int parameterIndex = 0;
            int arrayIndex = 0;
            int methodNameIndex = Array.IndexOf(methodArray, Convert.ToByte(36));
            byte[] processingArray = new byte[dataSize];
            Array.Copy(methodArray, processingArray, dataSize);
            string methodName = ASCIIEncoding.Default.GetString(methodArray, 0, methodNameIndex);
            Array.Resize(ref processingArray, dataSize - (methodNameIndex + 1));
            Array.Copy(methodArray, methodNameIndex + 1, processingArray, 0, dataSize - (methodNameIndex + 1));
            MethodInfo[] serverMethods = typeof(ServerMethods).GetMethods();
            MethodInfo selectedMethod = serverMethods.Where(x => x.Name == methodName).FirstOrDefault();
            ParameterInfo[] methodParameters = selectedMethod.GetParameters();
            int parameterLength;
            List<byte> parameterArrayList = new List<byte>();
            List<object> parametersObjectList = new List<object>();
            int loopStartIndex = 0;
            int furtheranceIndex = 0;

            while (loopStartIndex < methodParameters.Length)
            {
                parameterArrayList.Clear();
                parameterLength = processingArray[arrayIndex]; //parameterLength = 4

                while (arrayIndex < parameterLength + furtheranceIndex)
                {
                    parameterArrayList.Add(processingArray[arrayIndex+1]); //arrayIndex=0 //1 //2 //3
                    arrayIndex++;
                }
                if (methodParameters[parameterIndex].ParameterType.Name == "String")
                {
                    parametersObjectList.Add(ASCIIEncoding.Default.GetString(parameterArrayList.ToArray()));
                    parameterIndex++;
                }
                else
                {
                    parametersObjectList.Add(parameterArrayList[0]);
                }
                arrayIndex++;
                furtheranceIndex = arrayIndex;
                loopStartIndex++;
            }

            selectedMethod.Invoke(new ServerMethods(), parametersObjectList.ToArray());
           

        }

        private void SendMethodsInformationToClient(List<List<byte>> allServerMethods)
        {
            data = new byte[1024];
            List<byte> sendingMethods = new List<byte>();
            foreach (List<byte> item in allServerMethods)
            {
                sendingMethods.AddRange(item);
            }
            data = sendingMethods.ToArray();
            acceptedSocket.Send(data, 0, data.Length, 0);
        }
    }
}
