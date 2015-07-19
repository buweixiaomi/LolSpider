using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider.Unity
{
    public class Lib
    {
        public static string BytesToString(byte[] bs, string enco)
        {
            return System.Text.Encoding.GetEncoding(enco).GetString(bs);
        }

        
    }
}
