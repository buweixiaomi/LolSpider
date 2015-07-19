using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider.Unity
{
    public class HttpWebRequest
    {
        public static string Get(string url)
        {
            System.Net.WebRequest wr = System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = wr.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            List<byte> bs = new List<byte>();
            int b = -1;
            while ((b = stream.ReadByte()) != -1)
            {
                bs.Add((byte)b);
            }
            stream.Close();
            stream.Dispose();
            response.Close();
            return System.Text.UTF8Encoding.UTF8.GetString(bs.ToArray());
            return Lib.BytesToString(bs.ToArray(), "GBK");

        }
    }
}
