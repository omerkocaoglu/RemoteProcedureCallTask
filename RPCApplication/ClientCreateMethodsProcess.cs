using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class ClientCreateMethodsProcess
    {
        List<byte> method = new List<byte>();
        int parametersCount;
        string methodName = string.Empty;
        public ClientSideMethods Start(List<byte> method)
        {
            List<Enums.ParameterType> parameters = new List<Enums.ParameterType>();
            this.method = method;
            Enums.ReturnType methodReturnType = (Enums.ReturnType)Enum.Parse(typeof(Enums.ReturnType), method[0].ToString());
            parametersCount = Convert.ToInt32(method[1]);
            for (int i = 0; i < parametersCount; i++)
            {
                parameters.Add((Enums.ParameterType)Enum.Parse(typeof(Enums.ParameterType), method[1 + i+1].ToString()));
            }
            methodName = ASCIIEncoding.Default.GetString(method.ToArray(), (parametersCount+2), method.Count-(parametersCount+2));

            ClientSideMethods clientMethod = new ClientSideMethods();
            clientMethod.MethodName = methodName;
            clientMethod.MethodReturnType = methodReturnType;
            clientMethod.MethodsParameters = parameters;
            return clientMethod;

        }
    }
}
