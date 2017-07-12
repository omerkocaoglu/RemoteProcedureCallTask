using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class ClientSideMethods
    {
        public Enums.ReturnType MethodReturnType { get; set; }
        public List<Enums.ParameterType> MethodsParameters { get; set; }
        public string MethodName { get; set; }

        public List<object> MethodsParameterValues = new List<object>();
    }
}
