using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider.Models
{
    public class Player
    {
        public string servername { get; set; }
        public string playername { get; set; }
        public string parentplayer { get; set; }
        public int level { get; set; }
        public int fighting { get; set; }
        public int searchdeep { get; set; }
        public DateTime lastplaytime { get; set; }
    }
}
