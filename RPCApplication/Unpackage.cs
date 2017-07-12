using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class Unpackage
    {
        List<int> arrayMark = new List<int>();
        List<byte> methods = new List<byte>();
        List<List<byte>> allMethods = new List<List<byte>>();
        public Unpackage(List<byte> methods)
        {
            this.methods = methods;
        }
        public List<List<byte>> Start()
        {
            for (int i = 0; i < methods.Count; i++)
            {
                if (methods[i] == Convert.ToByte('$'))
                {
                    arrayMark.Add(i);
                }
            }
            for (int i = 0; i < arrayMark.Count; i++)
            {
                if (i == 0)
                {
                    allMethods.Add(methods.Take(arrayMark[i]).ToList());
                }
                else
                {
                    allMethods.Add(methods.Skip(arrayMark[i-1]+1).Take(arrayMark[i]-arrayMark[i-1]-1).ToList());
                }
            }
            return allMethods;
        }
    }
}
