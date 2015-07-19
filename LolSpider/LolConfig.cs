using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider
{
    public class LolConfig
    {
        //http://lolbox.duowan.com/matchList.php?serverName=%E7%94%B5%E4%BF%A1%E5%85%AD&playerName=%E4%B8%8D%E5%94%AF%E5%B0%8F%E7%B1%B3&page=2#13923564262
        public const string URL_MATCH_LIST = "http://lolbox.duowan.com/matchList.php?serverName={0}&playerName={1}&page={2}";
        public const string URL_MATCH_DETAILS = "http://lolbox.duowan.com/matchList/ajaxMatchDetail2.php?matchId={0}&serverName={1}&playerName={2}&favorate=0";
        public const string URL_PLAY_DETAILS = "http://lolbox.duowan.com/playerDetail.php?serverName={0}&playerName={1}";
    }
}
