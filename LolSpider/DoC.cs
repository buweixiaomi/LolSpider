using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider
{
    class DoC
    {
        public static List<System.Threading.Thread> Do(int searchdeep)
        {
            Unity.DbConn dbconn = new Unity.DbConn(".", "lolspider", "sa", "Xx~!@#");
            string server = "电信六";
            dbconn.Open();
            int total = 0;
            var users = DbVisiter.LolUser.GetListByPage(dbconn, server, searchdeep, 1, int.MaxValue / 2, out total);
            //Models.Player mine = new Models.Player() { fighting = 3720, searchdeep = 0, lastplaytime = DateTime.Now, level = 30, parentplayer = "", playername = "不唯小米", servername = server };

            //DbVisiter.LolUser.AddUser(dbconn, mine);
            List<System.Threading.Thread> threads = new List<System.Threading.Thread>();
            int tcount = 10;
            for (int i = 0; i < tcount; i++)
            {
                System.Threading.Thread tr = new System.Threading.Thread(() =>
                {
                    try
                    {
                        using (Unity.DbConn dbconn1 = new Unity.DbConn(".", "lolspider", "sa", "Xx~!@#"))
                        {
                            dbconn1.Open();
                            while (true)
                            {
                                Models.Player u = null;
                                lock (users)
                                {
                                    u = users.FirstOrDefault();
                                    if (u != null)
                                    {
                                        users.Remove(u);
                                    }
                                }

                                if (u == null)
                                {
                                    break;
                                }

                                var newact = new Actions.UseAction(dbconn1, server, u.playername, 50, u.searchdeep + 1);
                                newact.Do();
                            }
                        }
                    }
                    catch (Exception ex) { }
                });
                tr.IsBackground = true;
                tr.Start();
                threads.Add(tr);
            }
            return threads;
        }
    }
}
