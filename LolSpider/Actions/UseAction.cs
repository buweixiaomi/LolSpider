using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LolSpider.Actions
{
    public class UseAction
    {
        string _servername = "";
        string _playername = "";
        int _maxprematch = 0;
        int _searchdeep = 1;
        int currpno = 1;
        Unity.DbConn dbconn = null;
        public UseAction(Unity.DbConn dbc, string servername, string playername, int maxprematch, int searchdeep)
        {
            dbconn = dbc;
            _servername = servername;
            _playername = playername;
            _maxprematch = maxprematch;
            _searchdeep = searchdeep;
        }

        public List<Models.Player> Do()
        {
            List<string> str_users = new List<string>();
            while (currpno <= _maxprematch)
            {
                var x = GetPageMatchs(currpno);
                if (x == null || x.Count == 0)
                    break;
                foreach (var a in x)
                {
                    if (currpno > _maxprematch)
                        break;
                    var y = GetMatchUsers(a.matchid);
                    str_users.AddRange(y);
                }
                currpno++;
            }
            List<Models.Player> plers = new List<Models.Player>();
            foreach (string s in str_users)
            {
                if (DbVisiter.LolUser.ExistUser(dbconn, _servername, s))
                    continue;
                var tuser = GetUserInfo(s);
                if (tuser == null)
                    continue;
                plers.Add(tuser);
                try
                {
                    DbVisiter.LolUser.AddUser(dbconn, tuser);
                    Console.WriteLine(tuser.playername);
                }
                catch (Exception ex) { }

            }
            return plers;
        }

        public Models.Player GetUserInfo(string username)
        {
            string url = string.Format(LolConfig.URL_PLAY_DETAILS, _servername, username);
            string rhtml = Unity.HttpWebRequest.Get(url);
            var parser = Winista.Text.HtmlParser.Parser.CreateParser(rhtml, "utf8");
            Winista.Text.HtmlParser.Filters.HasAttributeFilter tnf = new Winista.Text.HtmlParser.Filters.HasAttributeFilter("class", "intro");

            var tb_a = parser.ExtractAllNodesThatMatch(tnf);
            if (tb_a.Count == 0)
                return null;
            Winista.Text.HtmlParser.ITag T1 = null;
            Winista.Text.HtmlParser.ITag T2 = null;
            Winista.Text.HtmlParser.ITag T3 = null;

            for (int i = 0; i < tb_a[0].Children.Count; i++)
            {
                var tg = tb_a[0].Children[i] as Winista.Text.HtmlParser.ITag;
                if (tg == null)
                    continue;
                if (tg.TagName == "DIV" && tg.GetAttribute("CLASS") == "avatar")
                {
                    T1 = tg;
                    continue;
                }
                if (tg.TagName == "DIV" && tg.GetAttribute("CLASS") == "text")
                {
                    T2 = tg;
                    continue;
                }
                if (tg.TagName == "DIV" && tg.GetAttribute("CLASS") == "fighting")
                {
                    T3 = tg;
                    continue;
                }
            }

            //deng ji
            int denji = 1;
            for (int i = 0; i < T1.Children.Count; i++)
            {
                //var tg = T1.Children[i] as Winista.Text.HtmlParser.ITag;
                //if (tg == null || tg.TagName != "EM")
                //    continue;
                if (T1.Children[i].GetType() == typeof(Winista.Text.HtmlParser.Nodes.TextNode))
                {
                    if (T1.Children[i].ToPlainTextString().Trim() == "")
                        continue;
                    int.TryParse(T1.Children[i].ToPlainTextString().Trim(), out denji);
                    break;
                }

            }

            int fighting = 0;
            DateTime uptime = DateTime.Parse("1900-1-1");
            for (int i = 0; i < T3.Children.Count; i++)
            {
                var tg = T3.Children[i] as Winista.Text.HtmlParser.ITag;
                if (tg == null || tg.TagName != "P")
                    continue;
                for (int k = 0; k < tg.Children.Count; k++)
                {
                    var tg_k = tg.Children[k] as Winista.Text.HtmlParser.ITag;
                    //if (tg_k == null || tg_k.TagName != "EM")
                    //    continue;
                    //for (int j = 0; j < tg_k.Children.Count; j++)
                    //{
                    //    var tg_k_j = tg_k.Children[j] as Winista.Text.HtmlParser.ITag;
                    //    if (tg_k_j == null || tg_k_j.TagName != "SPAN")
                    //        continue;
                    //    DateTime.TryParse((tg_k_j.GetAttribute("title") ?? "").Replace("更新时间：", ""), out uptime);
                    //    int.TryParse(tg_k_j.ToPlainTextString().Trim(), out fighting);
                    //}
                    if (tg_k == null || tg_k.TagName != "SPAN")
                        continue;

                    var tg_k_j = tg_k;
                    if (tg_k_j == null || tg_k_j.TagName != "SPAN")
                        continue;
                    DateTime.TryParse((tg_k_j.GetAttribute("title") ?? "").Replace("更新时间：", ""), out uptime);
                    int.TryParse(tg_k_j.ToPlainTextString().Trim(), out fighting);

                }
            }
            Models.Player u = new Models.Player();
            u.parentplayer = _playername;
            u.playername = username;
            u.fighting = fighting;
            u.level = denji;
            u.searchdeep = _searchdeep;
            u.servername = _servername;
            u.lastplaytime = uptime;
            return u;

        }

        public List<Models.Player> AysDo()
        {
            return null;
        }

        public List<string> GetMatchUsers(string matchid)
        {
            string url = string.Format(LolConfig.URL_MATCH_DETAILS, matchid, _servername, _playername);
            string rhtml = Unity.HttpWebRequest.Get(url);

            Winista.Text.HtmlParser.Filters.HasAttributeFilter tnf_a = new Winista.Text.HtmlParser.Filters.HasAttributeFilter("id", "zj-table--A");
            Winista.Text.HtmlParser.Filters.HasAttributeFilter tnf_b = new Winista.Text.HtmlParser.Filters.HasAttributeFilter("id", "zj-table--B");

            var tb_a =  Winista.Text.HtmlParser.Parser.CreateParser(rhtml, "utf8").ExtractAllNodesThatMatch(tnf_a);
            var tb_b = Winista.Text.HtmlParser.Parser.CreateParser(rhtml, "utf8").ExtractAllNodesThatMatch(tnf_b);
            List<string> us = new List<string>();
            us.AddRange(GetUsersFromTb(tb_a));
            us.AddRange(GetUsersFromTb(tb_b));
            return us;

        }

        public List<string> GetUsersFromTb(Winista.Text.HtmlParser.Util.NodeList nlist)
        {
            List<string> us = new List<string>();
            if (nlist == null || nlist.Count == 0)
                return us;
            Winista.Text.HtmlParser.INode tablenode = null;
            for (int i = 0; i < nlist[0].Children.Count; i++)
            {
                var tg = nlist[0].Children[i] as Winista.Text.HtmlParser.ITag;
                if (tg == null || tg.TagName != "TABLE")
                    continue;
                tablenode = nlist[0].Children[i];
                break;
            }
            if (tablenode == null)
                return us;
            Winista.Text.HtmlParser.Util.NodeList trs = null;
            for (int i = 0; i < tablenode.Children.Count; i++)
            {
                var tg = tablenode.Children[i] as Winista.Text.HtmlParser.ITag;
                if (tg == null)
                    continue;
                if (tg.TagName == "TBODY")
                {
                    trs = tg.Children;
                    break;
                }
                if (tg.TagName == "TR")
                {
                    trs = tablenode.Children;
                    break;
                }
            }

            if (trs == null || trs.Count == 0)
                return us;
            for (int i = 0; i < trs.Count; i++)
            {
                var tg = trs[i] as Winista.Text.HtmlParser.ITag;
                if (tg == null || tg.TagName != "TR")
                    continue;
                //get first td
                Winista.Text.HtmlParser.ITag firsttd = null;
                for (int k = 0; k < trs[i].Children.Count; k++)
                {
                    var td_t = trs[i].Children[k] as Winista.Text.HtmlParser.ITag;
                    if (td_t != null && td_t.TagName == "TD")
                    {
                        firsttd = td_t;
                        break;
                    }
                }
                if (firsttd == null)
                    continue;
                //get div
                Winista.Text.HtmlParser.ITag td_div = null;
                for (int k = 0; k < firsttd.Children.Count; k++)
                {
                    var td_t_div = firsttd.Children[k] as Winista.Text.HtmlParser.ITag;
                    if (td_t_div != null && td_t_div.TagName == "DIV")
                    {
                        td_div = td_t_div;
                        break;
                    }
                }
                if (td_div == null)
                    continue;
                //GET A
                Winista.Text.HtmlParser.ITag td_div_a = null;
                for (int k = 0; k < td_div.Children.Count; k++)
                {
                    var td_t_div = td_div.Children[k] as Winista.Text.HtmlParser.ITag;
                    if (td_t_div != null && td_t_div.TagName == "A")
                    {
                        td_div_a = td_t_div;
                        break;
                    }
                }
                if (td_div_a == null)
                    continue;
                us.Add(td_div_a.ToPlainTextString().Trim());
            }
            return us;

        }


        private List<PageMatch> GetPageMatchs(int pno)
        {
            List<PageMatch> ttt = new List<PageMatch>();
            string url = string.Format(LolConfig.URL_MATCH_LIST, _servername, _playername, pno);
            string rhtml = Unity.HttpWebRequest.Get(url);

            var parser = Winista.Text.HtmlParser.Parser.CreateParser(rhtml, "utf8");
            Winista.Text.HtmlParser.Filters.HasAttributeFilter tnf = new Winista.Text.HtmlParser.Filters.HasAttributeFilter();
            tnf.AttributeName = "class";
            tnf.AttributeValue = "l-box";
            var divlist = parser.ExtractAllNodesThatMatch(tnf);
            if (divlist.Count > 0)
            {
                var matchs = divlist[0].FirstChild.Children;
                for (int i = 0; i < matchs.Count; i++)
                {
                    Winista.Text.HtmlParser.ITag it = matchs[i] as Winista.Text.HtmlParser.ITag;
                    if (it == null)
                        continue;
                    if (it.ToHtml().ToString() == "")
                        continue;
                    string matchid_html = it.GetAttribute("id");
                    if (matchid_html != null && matchid_html.StartsWith("cli"))
                    {
                        string matchid_str = matchid_html.Substring(3);
                        for (int k = 0; k < matchs[i].Children.Count; k++)
                        {
                            var p_tag = matchs[i].Children[k] as Winista.Text.HtmlParser.ITag;
                            if (p_tag == null || p_tag.ToHtml().Trim() == "")
                                continue;
                            if (p_tag.TagName == "P" && p_tag.GetAttribute("class") == "info")
                            {
                                for (int j = 0; j < matchs[i].Children[k].Children.Count; j++)
                                {
                                    if (matchs[i].Children[k].Children[j].GetType() == typeof(Winista.Text.HtmlParser.Nodes.TextNode))
                                    {
                                        string dtstr = matchs[i].Children[k].Children[j].ToPlainTextString().Replace("&nbsp;", "").Trim();
                                        if (dtstr != "")
                                        {
                                            ttt.Add(new PageMatch() { matchid = matchid_str, time = dtstr });
                                            break;
                                        }
                                    }
                                }
                                break;
                            }

                        }
                    }

                }
            }
            return ttt;
        }

        class PageMatch
        {
            public string time { get; set; }
            public string matchid { get; set; }
        }
    }


}
