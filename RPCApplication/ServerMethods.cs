using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCApplication
{
    public class ServerMethods
    {
        [RPC]
        public void Welcome(string value)
        {
            Console.WriteLine("Welcome, {0}", value);
        }

        [RPC]
        public void Getdate()
        {
            Console.WriteLine("Today's: {0}", DateTime.Now.ToShortDateString());
        }

        [RPC]
        public void Gettime()
        {
            Console.WriteLine("Today's: {0}", DateTime.Now.ToShortTimeString());
        }

        [RPC]
        public void Squarearea(byte edge)
        {
            Console.WriteLine("Square Area: {0}", (edge * edge).ToString());
        }

        [RPC]
        public void Trianglearea(byte baseEdge, byte height)
        {
            Console.WriteLine("Triangle Area. {0}", ((baseEdge * height) / 2).ToString());
        }

        [RPC]
        public void Getdayofweek()
        {
            Console.WriteLine("Today's: {0}",DateTime.Today.ToString());
        }

        [RPC]
        public void Deneme(string ad, string soyad)
        {
            Console.WriteLine("Hello " + ad + " " + soyad);
        }
        [RPC]
        public void Sonradaneklenen1()
        {
            Console.WriteLine("Sonradan Eklenen Metot 1");
        }

        [RPC]
        public void Sonradaneklenen2()
        {
            Console.WriteLine("Sonradan Eklenen Metot 2");
        }

        [RPC]
        public void Sonradaneklenen3()
        {
            Console.WriteLine("Sonradan Eklenen Metot 3");
        }

        [RPC]
        public void Sonradaneklenen4(string ad, string soyad)
        {
            Console.WriteLine(string.Format("Merhaba {0} {1}, Bu Sonradan Eklenen Metot 4",ad,soyad));
        }

        public void Newmethod1()
        {
            Console.WriteLine("Bu metot RPC attribute sahip değildir.");
        }
    }
}
