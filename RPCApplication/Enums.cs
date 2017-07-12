using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class Enums
    {
        public enum ReturnType:byte
        {
            Void,
            Byte,
            Int,
            Double,
            Decimal,
            String,
            Datetime,
        }

        public enum ParameterType:byte
        {
            None,
            Byte,
            Short,
            Int,
            Long,
            Decimal,
            Double,
            String
        }
    }
}
