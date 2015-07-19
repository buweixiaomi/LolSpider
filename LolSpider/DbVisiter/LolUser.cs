using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider.DbVisiter
{
    public class LolUser
    {
        public static bool ExistUser(Unity.DbConn dbconn, string server, string playername)
        {
            int i = dbconn.ExecScaner<int>("select count(1) from lolplayer where servername=@servername and playername=@playername", new
            {
                playername = playername,
                servername = server
            });
            return i > 0;
        }

        public static void AddUser(Unity.DbConn dbconn, Models.Player usermodel)
        {
            int i = dbconn.ExecSql("insert into lolplayer(servername,playername,parentplayer,[level],fighting,searchdeep,lastplaytime) values(@servername,@playername,@parentplayer,@level,@fighting,@searchdeep,@lastplaytime)", usermodel);
        }

        public static Models.Player Get(Unity.DbConn dbconn, string server, string playername)
        {
            var model = dbconn.ExecModel<Models.Player>("select * from lolplayer where servername=@servername and playername=@playername", new
            {
                playername = playername,
                servername = server
            });
            if (model.Count == 0)
                return null;
            return model[0];
        }

        public static List<Models.Player> GetListByPage(Unity.DbConn dbconn, string server, int searchdeep, int pno, int pagesize, out int total)
        {
            total = dbconn.ExecScaner<int>("select count(1) from lolplayer where  servername=@servername and [searchdeep]=@searchdeep", new { servername = server, searchdeep = searchdeep });
            string pagesql = string.Format("select * from (select  ROW_NUMBER() over (order by playername asc) as rownum,* from lolplayer where  servername=@servername and [searchdeep]=@searchdeep) A where A.rownum between {0} and {1}", (pno - 1) * pagesize + 1, pno * pagesize);
            var model = dbconn.ExecModel<Models.Player>(pagesql, new
            {
                servername = server,
                searchdeep = searchdeep
            });
            return model;
        }
    }
}
