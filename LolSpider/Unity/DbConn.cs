using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace LolSpider.Unity
{
    public class DbConn : IDisposable
    {

        private System.Data.SqlClient.SqlConnection conn = null;
        private SqlTransaction trans = null;
        public DbConn(string dbsou, string dbname, string loginid, string pwd)
        {
            string connstr = string.Format("server={0};database={1};uid={2};pwd={3}", dbsou, dbname, loginid, pwd);
            conn = new SqlConnection(connstr);
        }
        public DbConn(string connstr)
        {
            conn = new SqlConnection(connstr);
        }

        public void Open()
        {
            conn.Open();
        }

        public SqlConnection getBaseConn()
        {
            return conn;
        }

        public string GetDbname()
        {
            return conn.Database;
        }
        public void BeginTrans()
        {
            trans = conn.BeginTransaction();
        }

        public void Commit()
        {
            trans.Commit();
        }

        public void RollBack()
        {
            trans.Rollback();
        }
        public static DbConn CreateConn(string str)
        {
            return new DbConn(str);
        }

        public int ExecSql(string sql, object para)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(getparaFromObj(para));
            cmd.CommandTimeout = 30 * 60;
            int r = cmd.ExecuteNonQuery();
            return r;
        }

        public int ExecSql(string sql, object para,int minutes)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(getparaFromObj(para));
            cmd.CommandTimeout = minutes * 60;
            int r = cmd.ExecuteNonQuery();
            return r;
        }

        public object ExecScaner(string sql, object para)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(getparaFromObj(para));
            object r = cmd.ExecuteScalar();
            return r;
        }
        public T ExecScaner<T>(string sql, object para)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(getparaFromObj(para));
            object obj = cmd.ExecuteScalar();
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        public DataTable ExecTable(string sql, object para)
        {
            SqlDataAdapter sqlad = new SqlDataAdapter();
            sqlad.SelectCommand = new SqlCommand(sql, conn);
            sqlad.SelectCommand.Parameters.AddRange(getparaFromObj(para));
            DataTable tb = new DataTable();
            sqlad.Fill(tb);
            return tb;
        }

        public List<T> ExecModel<T>(string sql, object para)
        {
            return ExecModel<T>(sql, para, (new Func<DataRow, T>(DefaultFillAct<T>)));
        }

        public List<T> ExecModel<T>(string sql, object para, Func<DataRow, T> fillact)
        {
            List<T> listmodel = new List<T>();
            DataTable tb = ExecTable(sql, para);
            foreach (DataRow dr in tb.Rows)
            {
                listmodel.Add(fillact(dr));
            }
            return listmodel;
        }

        public static T DefaultFillAct<T>(DataRow dr)
        {
            T t = default(T);
            System.Reflection.ConstructorInfo[] cons = typeof(T).GetConstructors();

            foreach (var a in cons)
            {
                System.Reflection.ParameterInfo[] paras = a.GetParameters();
                if (paras == null || paras.Length == 0)
                {
                    t = (T)Activator.CreateInstance(typeof(T), null);
                    System.Reflection.PropertyInfo[] props = t.GetType().GetProperties();
                    foreach (var p in props)
                    {
                        if (ContainPropInTable(dr.Table, p.Name))
                        {
                            if (p.PropertyType == typeof(string))
                            {
                                p.SetValue(t, dr[p.Name].ToString(), null);
                            }
                            else if (p.PropertyType == typeof(int))
                            {
                                p.SetValue(t, Convert.ToInt32(dr[p.Name]), null);
                            }
                            else if (p.PropertyType == typeof(Int64))
                            {
                                p.SetValue(t, Convert.ToInt64(dr[p.Name]), null);
                            }
                            else if (p.PropertyType == typeof(decimal))
                            {
                                p.SetValue(t, Convert.ToDecimal(dr[p.Name]), null);
                            }
                            else if (p.PropertyType == typeof(double))
                            {
                                p.SetValue(t, Convert.ToDouble(dr[p.Name]), null);
                            }
                            else if (p.PropertyType == typeof(DateTime))
                            {
                                p.SetValue(t, Convert.ToDateTime(dr[p.Name]), null);
                            }
                            else
                            {
                                p.SetValue(t, dr[p.Name], null);
                            }
                        }
                    }
                    //if (containmembers)
                    //{
                    //    System.Reflection.MemberInfo[] membs = t.GetType().GetMembers();
                    //    foreach (var m in membs)
                    //    {
                    //        if (ContainPropInTable(dr.Table, m.Name))
                    //        {
                    //            m.va(t, dr[m.Name]);
                    //        }
                    //    }
                    //}
                    return t;
                }
                if (paras.Length == 1 || paras[0].ParameterType == typeof(DataRow))
                {
                    t = (T)Activator.CreateInstance(typeof(T), dr);
                    return t;
                }
            }
            throw new Exception("no required constructor");
        }

        private static bool ContainPropInTable(DataTable tb, string colname, bool ignorecase = true)
        {
            if (ignorecase)
            {
                foreach (DataColumn c in tb.Columns)
                {
                    if ((c.ColumnName ?? "").ToLower() == colname.ToLower())
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (DataColumn c in tb.Columns)
                {
                    if ((c.ColumnName ?? "") == colname)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Close()
        {
            conn.Close();
            conn.Dispose();
        }

        public void Dispose()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public static SqlParameter[] getparaFromObj(object obj)
        {
            List<SqlParameter> paralist = new List<SqlParameter>();
            if (obj == null)
                return paralist.ToArray();
            foreach (var p in obj.GetType().GetProperties())
            {
                object v = p.GetValue(obj, null);
                SqlParameter para = new SqlParameter(p.Name, v);
                paralist.Add(para);
            }
            return paralist.ToArray();
        }
    }
}
