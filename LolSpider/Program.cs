using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider
{
    class Program
    {
        static void Main(string[] args)
        {

            var ts = DoC.Do(2);
            foreach (var a in ts)
            {
                a.Join();
            }
            Console.WriteLine("2");
            ts = DoC.Do(3);
            foreach (var a in ts)
            {
                a.Join();
            }

            Console.WriteLine("3");
            ts = DoC.Do(4);
            foreach (var a in ts)
            {
                a.Join();
            }

            Console.WriteLine("4");
            ts = DoC.Do(5);
            foreach (var a in ts)
            {
                a.Join();
            }

            Console.WriteLine("5");

            ts = DoC.Do(6);
            foreach (var a in ts)
            {
                a.Join();
            }


            Console.WriteLine("6");
            ts = DoC.Do(7);
            foreach (var a in ts)
            {
                a.Join();
            }

            Console.WriteLine("7");

            ts = DoC.Do(8);
            foreach (var a in ts)
            {
                a.Join();
            }
            Console.WriteLine("8");

        }
    }
}
