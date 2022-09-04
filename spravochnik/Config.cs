using System.Text;
using System.Collections;
using Nwuram.Framework.Data;
using Nwuram.Framework.Settings.Connection;
using System.Data;
using System;
using Nwuram.Framework.Settings.User;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace spravochnik
{
    class Config
    {
        public static readonly string PathFile = Application.StartupPath;

        public static Settings ProgSettngs;

        public static Procedures hCntMain { get; set; } //осн. коннект

        public static Procedures hCntMainKassRealiz { get; set; } //осн. коннект

        public static Procedures hCntSecond { get; set; } //осн. коннект

        public static void DoOnUIThread(MethodInvoker d, Form _this)
        {
            if (_this.InvokeRequired) { _this.Invoke(d); } else { d(); }
        }

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
                    newString += buffString + "\n";
                }
                else
                {
                    buffString = str.Substring(arra[i - 1], arra[i]);
                    buffString = buffString.PadLeft(Convert.ToInt32(buffString.Length + ((maxLength - (arra[i] - arra[i - 1] - 1)) / 2) * 1.8));
                    newString = buffString + "\n";
                }
            }
            return newString;
        }
    }

    public class Settings
    {
        public List<int> IdTerminal { set; get; }
    }

    class Procedures : SqlProvider
    {
        public Procedures(string server, string database, string username, string password, string appName)
              : base(server, database, username, password, appName)
        {
        }
        ArrayList ap = new ArrayList();

        public DataTable getListGoodsDbase1(bool isVOO)
        {
            ap.Clear();
            ap.Add(isVOO);
            DataTable dtResult = executeProcedure("[sendFrontol].[getListGoodsDbase1]",
                 new string[1] {"@isVOO" },
                 new DbType[1] {DbType.Boolean }, ap);
            return dtResult;
        }

        public DataTable getListGoodsKassRealiz()
        {
            ap.Clear();

            DataTable dtResult = executeProcedure("[sendFrontol].[getListGoodsKassRealiz]",
                 new string[0] { },
                 new DbType[0] { }, ap);

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


        public async Task<DataTable> setGoodvsMarking(string jsone, int id_grp1,bool isInclude)
        {
            ap.Clear();
            ap.Add(jsone);
            ap.Add(id_grp1);
            ap.Add(isInclude);
            DataTable dtResult = executeProcedure("[sendFrontol].[setGoodvsMarking]",
                 new string[3] { "@jsone", "@id_grp1", "@isInclude" },
                 new DbType[3] { DbType.String, DbType.Int32, DbType.Boolean }, ap);
            return dtResult;
        }

        #endregion
    }

    enum logEvents
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

        public static void SetWatermark(this System.Windows.Forms.TextBox textBox, string watermarkText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, watermarkText);
        }

    }
}
