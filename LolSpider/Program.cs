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

            for (int lev = 0; lev < 10; lev++)
            {
                var ts = DoC.Do(lev);
                foreach (var a in ts)
                {
                    a.Join();
                }
                Console.WriteLine(lev);
            }
            Console.WriteLine("OK");
            Console.Read();


        }
    }
}
