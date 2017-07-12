using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class Package
    {
        byte parametersCount;
        string methodName = string.Empty;
        
        public List<byte> CompoundMethods(MethodInfo packagingMethod)  //creating merhod byte array
        {
            MethodInfo rpcMethod = packagingMethod;
            Enums.ReturnType methodReturnType = (Enums.ReturnType)Enum.Parse(typeof(Enums.ReturnType), rpcMethod.ReturnType.Name);
            ParameterInfo[] methodParameters = rpcMethod.GetParameters();
            parametersCount = Convert.ToByte(methodParameters.Length);
            methodName = rpcMethod.Name;
            List<byte> methodByteArray = new List<byte>();
            ConvertMethodReturnType(methodByteArray, methodReturnType);
            ConvertMethodParametersCount(methodByteArray, parametersCount);
            ConvertMethodParameters(methodByteArray, methodParameters);
            ConvertMethodName(methodByteArray, methodName);
            return methodByteArray;
        }
        private void ConvertMethodParameters(List<byte> array, ParameterInfo[] methodsParameters)
        {
            foreach (var item in methodsParameters)
            {
                array.Add((byte)((Enums.ParameterType)Enum.Parse(typeof(Enums.ParameterType), item.ParameterType.Name)));
            }
        }
        public void ConvertMethodParametersCount(List<byte> array, byte parametersCount)
        {
            array.Add(parametersCount);
        }
        public void ConvertMethodReturnType(List<byte> array, Enums.ReturnType methodReturnType)
        {
            array.Add((byte)methodReturnType);
        }
        public void ConvertMethodName(List<byte> array, string methodName)
        {
            array.AddRange(ASCIIEncoding.Default.GetBytes(methodName));
            array.Add(Convert.ToByte('$'));
        }
    }
}
