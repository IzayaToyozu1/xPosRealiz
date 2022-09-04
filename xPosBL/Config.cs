using System.Collections;
using Nwuram.Framework.Data;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace xPosBL
{
    public class Config
    {
        public static Procedures hCntMain { get; set; } //осн. коннект
        public static Procedures hCntMainKassRealiz { get; set; } //осн. коннект
        public static Procedures hCntSecond { get; set; } //осн. коннект

        public static string centralText(string str)
        {
            int[] arra = new int[255];
            int count = 0;
            int maxLength = 0;
            int indexF = -1;
            arra[count] = 0;
            count++;
            indexF = str.IndexOf("\n");
            arra[count] = indexF;
            while (indexF != -1)
            {
                count++;
                indexF = str.IndexOf("\n", indexF + 1);
                arra[count] = indexF;
            }
            maxLength = arra[1] - arra[0];
            for (int i = 1; i < count; i++)
            {
                if (maxLength < (arra[i] - arra[i - 1]))
                {

                    maxLength = arra[i] - arra[i - 1];
                    if (i >= 2)
                    {
                        maxLength = maxLength - 1;
                    }
                }
            }
            string newString = "";
            string buffString = "";
            for (int i = 1; i < count; i++)
            {
                if (i >= 2)
                {

                    buffString = str.Substring(arra[i - 1] + 1, (arra[i] - arra[i - 1] - 1));
                    buffString = buffString.PadLeft(Convert.ToInt32(buffString.Length + ((maxLength - (arra[i] - arra[i - 1] - 1)) / 2) * 1.8));
                    //    buffString = buffString.PadRight(buffString.Length + ((maxLength - (arra[i] - arra[i - 1] - 1)) / 2)*2);
                    newString += buffString + "\n";
                }
                else
                {
                    buffString = str.Substring(arra[i - 1], arra[i]);
                    buffString = buffString.PadLeft(Convert.ToInt32(buffString.Length + ((maxLength - (arra[i] - arra[i - 1] - 1)) / 2) * 1.8));
                    // buffString = buffString.PadRight(buffString.Length + ((maxLength - (arra[i] - arra[i - 1])) / 2)*2);
                    newString = buffString + "\n";
                }

            }

            return newString;
        }
    }

    public class Procedures : SqlProvider
    {
        public Procedures(string server, string database, string username, string password, string appName)
              : base(server, database, username, password, appName)
        {
        }
        private readonly ArrayList ap = new ArrayList();

        public DataTable getListGoodsDbase1(bool isVOO)
        {
            ap.Clear();
            ap.Add(isVOO);
            DataTable dtResult = executeProcedure("[sendFrontol].[getListGoodsDbase1_XPos_V3]",
                 new string[1] {"@isVOO" },
                 new DbType[1] {DbType.Boolean }, ap);
            //TODO: Изменил процедуру
            return dtResult;
        }
        public DataTable getListGoodsKassRealiz(int lastIdGoods)
        {
            ap.Clear();
            ap.Add(lastIdGoods);
            DataTable dtResult = executeProcedure("[sendFrontol].[getListGoodsKassRealiz_XPos_V3]",
                 new string[] { "@LastIdGoods" },
                 new DbType[] { DbType.Int32 }, ap); //TODO: Изменил процедуру

            return dtResult;
        }

        public DataTable GetLastIdGoodTerminal()
        {
            ap.Clear();
            DataTable dtResult = executeProcedure("[sendFrontol].[GetLastIdGoodTerminal_XPos_V3]",
                 new string[] { },
                 new DbType[] { }, ap); //TODO: Изменил процедуру
            return dtResult;
        }

        #region "Справочник Сервисов"

        public async Task<DataTable> getTypeMarking(bool withAllDeps = false)
        {
            ap.Clear();

            DataTable dtResult = executeProcedure("[sendFrontol].[getTypeMarking]",
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

                    row["cName"] = "Все типы";
                    row["id"] = 0;
                    row["isMain"] = 0;
                    dtResult.Rows.Add(row);
                    dtResult.AcceptChanges();
                    dtResult.DefaultView.Sort = "isMain asc, cName asc";
                    dtResult = dtResult.DefaultView.ToTable().Copy();
                }
            }

            return dtResult;
        }

        public async Task<DataTable> setTypeMarking(int id, string cName, int typeXpos, int result, bool isDel)
        {
            ap.Clear();

            ap.Add(id);
            ap.Add(cName);
            ap.Add(typeXpos);
            ap.Add(result);
            ap.Add(isDel);

            DataTable dtResult = executeProcedure("[sendFrontol].[setTypeMarking]",
                 new string[5] { "@id", "@cName", "@typeXpos", "@result", "@isDel" },
                 new DbType[5] { DbType.Int32, DbType.String, DbType.Int32, DbType.Int32, DbType.Boolean }, ap);

            return dtResult;
        }

        public async Task<DataTable> getDeps(bool withAllDeps = false)
        {
            ap.Clear();

            DataTable dtResult = executeProcedure("[sendFrontol].[getDeps]",
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

                    row["cName"] = "Все Отделы";
                    row["id"] = 0;
                    row["isMain"] = 0;
                    dtResult.Rows.Add(row);
                    dtResult.AcceptChanges();
                    dtResult.DefaultView.Sort = "isMain asc, id asc";
                    dtResult = dtResult.DefaultView.ToTable().Copy();
                }
            }
            else
            {
                dtResult.DefaultView.Sort = "id asc";
                dtResult = dtResult.DefaultView.ToTable().Copy();
            }

            return dtResult;
        }

        public async Task<DataTable> getGrp(bool withAllDeps = false)
        {
            ap.Clear();
            DataTable dtResult = executeProcedure("[sendFrontol].[getGrp]",
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

                    row["cName"] = "Все группы";
                    row["id"] = 0;
                    row["isMain"] = 0;
                    dtResult.Rows.Add(row);
                    dtResult.AcceptChanges();
                    dtResult.DefaultView.Sort = "isMain asc, cName asc";
                    dtResult = dtResult.DefaultView.ToTable().Copy();
                }
            }
            else
            {
                dtResult.DefaultView.Sort = "cName asc";
                dtResult = dtResult.DefaultView.ToTable().Copy();
            }

        

            return dtResult;
        }

        public async Task<DataTable> setGrp1VsTypeMarking(int id, int id_grp1, int id_typeXposMark,bool is_CheckMarking, int result, bool isDel)
        {
            ap.Clear();

            ap.Add(id);
            ap.Add(id_grp1);
            ap.Add(id_typeXposMark);
            ap.Add(is_CheckMarking);
            ap.Add(result);
            ap.Add(isDel);

            DataTable dtResult = executeProcedure("[sendFrontol].[setGrp1VsTypeMarking]",
                 new string[6] { "@id", "@id_grp1", "@id_typeXposMark", "@is_CheckMarking", "@result", "@isDel" },
                 new DbType[6] { DbType.Int32, DbType.Int32, DbType.Int32,DbType.Boolean, DbType.Int32, DbType.Boolean }, ap);

            return dtResult;
        }

        public async Task<DataTable> getGrp1VsTypeMarking()
        {
            ap.Clear();

            DataTable dtResult = executeProcedure("[sendFrontol].[getGrp1VsTypeMarking]",
                 new string[0] { },
                 new DbType[0] { }, ap);

            return dtResult;
        }

        public async Task<DataTable> getGoodvsMarking()
        {
            ap.Clear();

            DataTable dtResult = executeProcedure("[sendFrontol].[getGoodvsMarking]",
                 new string[0] { },
                 new DbType[0] { }, ap);

            return dtResult;
        }
        #endregion
    }

    public enum logEvents
    {
        Добавление_фирмы = 1469,
        Редактирование_фирмы = 1470,
        Удаление_фирмы = 1471,
        Добавление_сервиса = 1635,
        Редактирование_сервиса = 1636,
        Удаление_сервиса = 1637,
        Выгрузка_отчёта = 79,
        Добавление_фирмы_сервис = 1638,
        Редактирование_фирмы_сервис = 1639,
        Удаление_фирмы_сервис = 1640,
        Добавление_оплаты = 181,
        Редактирование_оплаты = 1641,
        Удаление_оплаты = 182
    }

    public static class TextBoxWatermarkExtensionMethod
    {
        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = ECM_FIRST + 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
    }
}
