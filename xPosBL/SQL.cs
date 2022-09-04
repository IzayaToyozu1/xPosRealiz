using System;
using System.Collections;
using Nwuram.Framework.Data;
using Nwuram.Framework.Settings.Connection;
using System.Data;

namespace xPosBL
{
    public class SQL
    {
        static readonly ArrayList ap = new ArrayList();
        static readonly SqlProvider sql = new SqlProvider(ConnectionSettings.GetServer(), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
        static readonly SqlProvider sql2 = new SqlProvider(ConnectionSettings.GetServer("3"), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);

        #region Sprav
        public static DataTable getListDeps()
        {
            ap.Clear();
            return sql.executeProcedure("[sendFrontol].[getListDeps]",
                new string[] { },
                new DbType[] { }, ap);
        }

        public static DataTable getListTovar()
        {
            ap.Clear();
            return sql.executeProcedure("[sendFrontol].[getListTovar]",//for 5.50 dbase1
                new string[] { },
                new DbType[] { }, ap);
        }
       
        public static DataTable getListTovarVVO()
        {
            ap.Clear();
            return sql2.executeProcedure("[sendFrontol].[getListTovar]",
                new string[] { },
                new DbType[] { }, ap);
        }

        public static DataTable getListGrp()
        {
            ap.Clear();
            return sql.executeProcedure("[sendFrontol].[getListGrp]",
                new string[] { },
                new DbType[] { }, ap);
        }

        public static long getLastIdSprav()
        {
            ap.Clear();
            DataTable dt = sql.executeProcedure("[sendFrontol].[getLastIdSprav]",
                new string[] { },
                new DbType[] { }, ap);
            try
            {
               return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            catch { return 0; }
        }

        public static DataTable getLastId()
        {
            ap.Clear();
            return sql.executeProcedure("[sendFrontol].[getLastID]",
                new string[] { },
                new DbType[] { }, ap);
        }

        public static DataTable setLastId()
        {
            return setLastId(0, 0);
        }

        public static DataTable setLastId(int termNum, long id)
        {
            ap.Clear();
            ap.Add(termNum);
            ap.Add(id);
            ap.Add(Nwuram.Framework.Settings.User.UserSettings.User.Id); //TODO измененная процедура
            return sql.executeProcedure("[sendFrontol].[setLastID_XPos_V3]",
                new string[] { "@number", "@lastIDTerminal", "@id_user" },
                new DbType[] { DbType.Int32, DbType.Int64,DbType.Int32 }, ap);
        }

        public static DataTable setJSprav(int terminal, long id)
        {
            ap.Clear();
            ap.Add(terminal);
            ap.Add(id);
            return sql.executeProcedure("[sendFrontol].[setJSprav]",
                new string[] { "@terminal", "@id" },
                new DbType[] { DbType.Int32, DbType.Int64 }, ap);
        }

        public static string getJSprav(int terminal)
        {
            ap.Clear();
            ap.Add(terminal);
            DataTable dt = sql.executeProcedure("[sendFrontol].[getJSprav]",
                new string[] { "@terminal"},
                new DbType[] { DbType.Int32}, ap);
            try { return dt.Rows[0][0].ToString(); }
            catch { return "0"; }
        }

        #endregion

        public static DataTable getSettings(string id_value)
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(id_value);
            return sql.executeProcedure("[sendFrontol].[getSettings]",
                new string[2] { "@id_prog", "@id_value" },
                new DbType[2] { DbType.Int32,DbType.String }, ap);
            
        }

        public static DataTable setSettings(string id_value, string value)
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(id_value);
            ap.Add(value);

            return sql.executeProcedure("[sendFrontol].[setSettings]",
                 new string[3] { "@id_prog", "@id_value", "@value" },
                 new DbType[3] { DbType.Int32, DbType.String, DbType.String }, ap);
        }

        public static DataTable getCatalogPromotionalTovars()
        {
            ap.Clear();

            return sql.executeProcedure("[xpos].[getCatalogPromotionalTovars]",
                new string[0] { },
                new DbType[0] { }, ap);

        }

        public static DataTable GetTerminalType(bool withAllDeps = false)
        {
            ap.Clear();

            DataTable dtResult = sql.executeProcedure("[sendFrontol].[getTerminalType]",
                 new string[0] { },
                 new DbType[0] { }, ap);

            if (withAllDeps)
            {
                if (dtResult != null)
                {
                    if (!dtResult.Columns.Contains("isMain"))
                    {
                        DataColumn col = new DataColumn("isMain", typeof(int));
                        col.DefaultValue = 1;
                        dtResult.Columns.Add(col);
                        dtResult.AcceptChanges();
                    }

                    DataRow row = dtResult.NewRow();

                    row["NameTerminalType"] = "Все Типы";
                    row["id"] = 0;
                    row["isMain"] = 0;
                    dtResult.Rows.Add(row);
                    dtResult.AcceptChanges();
                    dtResult.DefaultView.Sort = "isMain asc, NameTerminalType asc";
                    dtResult = dtResult.DefaultView.ToTable().Copy();
                }
            }
            else
            {
                dtResult.DefaultView.Sort = "NameTerminalType asc";
                dtResult = dtResult.DefaultView.ToTable().Copy();
            }

            return dtResult;
        }

        public static DataTable GetSpravTerminal()
        {
            ap.Clear();

            return sql.executeProcedure("[sendFrontol].[GetSpravTerminal_XPos_V3]", //TODO 
                new string[0] { },
                new DbType[0] { }, ap);
        }
        public static DataTable GetPromTovar()
        {
            ap.Clear();
            return sql.executeProcedure("[sendFrontol].[GetPromotionalTovars]",
                    new string[0] { },
                    new DbType[0] { }, ap);
        }

        public static DataTable GetPosts(int idPost)
        {
            ap.Clear();
            ap.Add(idPost);
            return sql.executeProcedure("[sendFrontol].[GetPosts]",
                new[] { "@idPost" },
                new[] { DbType.Int32 }, ap);
        }

        public static DataTable GetPostsVvo(int idPost)
        {
            ap.Clear();
            ap.Add(idPost);
            return sql2.executeProcedure("[sendFrontol].[GetPosts]",
                new[] { "@idPost" },
                new[] { DbType.Int32 }, ap);
        }
    }
}