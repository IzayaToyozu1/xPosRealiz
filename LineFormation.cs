using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

namespace xPosBL.GoodsDirectories.CreateSprav
{
    public class LineFormation
    {
        internal List<int> listGrp;
        internal DataTable dtPromoGoods;
        internal DataTable TypeMarking;
        internal DataTable GoodMarking;
        internal List<string> listPromoGoods;
        internal List<int> listIDPromoGoods;

        public LineFormation()
        {
            listGrp = new List<int>();
            dtPromoGoods = new DataTable();
            TypeMarking = new DataTable();
            GoodMarking = new DataTable();
            listPromoGoods = new List<string>();
            listIDPromoGoods = new List<int>();
        }

        private void InitListGrpSettings()
        {
            listGrp.Clear();
            DataTable dtTmp = SQL.getSettings("igr1");

            if (dtTmp != null && dtTmp.Rows.Count > 0)
                foreach (DataRow row in dtTmp.Rows)
                {
                    int idGrp;
                    if (int.TryParse(row["value"].ToString(), out idGrp))
                        listGrp.Add(idGrp);
                }
        }

        private void InitPromoGoods()
        {
            listPromoGoods.Clear();
            listIDPromoGoods.Clear();
            dtPromoGoods = SQL.getCatalogPromotionalTovars();

            TypeMarking = Config.hCntMain.getGrp1VsTypeMarking().Result;
            GoodMarking = Config.hCntMain.getGoodvsMarking().Result;
            
        }

        internal string StringInserGRP(DataRow rDep, string colId, string colName)
        {
            return StringInserGRP(rDep, colId, colName, "");
        }

        internal string StringInserGRP(DataRow rDep, string colId, string colName, string colIdParent)
        {
            string id = rDep[colId].ToString();
            string name = rDep[colName].ToString();
            string str = id + ";;" + name + ";" + name + ";;;;;;;;;;;;" + colIdParent + ";0;";
            return str;
        }

        internal void Update()
        {
            InitListGrpSettings();
            InitPromoGoods();
        } //

        internal string StringInserTovar(DataRowView row)
        {
            string id_tovar = row["id_tovar"].ToString();
            string ean = row["ean"].ToString().Trim();
            string name = row["name"].ToString().Replace(";", " ");
            string price = row["price"].ToString();
            string grp = row["grp"].ToString();
            string kodVVO = row["kodVVO"].ToString();
            string firm = row["firm"].ToString();
            string id_post = row["id_post"].ToString();
            string weight = row["ean"].ToString().Trim().Length == 4 ? "1" : "0";
            string id_unit = row["id_unit"].ToString();

            string[] good = GetItemSetting();
            if (good.Contains(row["ean"].ToString().Trim()))
                weight = "1";

            string tax = string.Empty;
            switch (row["tax"].ToString())
            {
                case "18": tax = "1"; break;
                case "20": tax = "1"; break;
                case "10": tax = "2"; break;
                default:
                    {
                        if (!File.Exists(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
                            using (FileStream fs = File.Create(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
                            {
                                fs.Close();
                            }

                        using (StreamWriter sw = new StreamWriter(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt", true))
                        {
                            sw.WriteLine("------------------error");
                            sw.WriteLine("id_tovar = " + id_tovar + ";ean = " + ean + ";name = " + name + ";price = " + price + ";tax = " + row["tax"].ToString());
                            sw.WriteLine("------------------");
                            sw.Close();

                        }
                        tax = "3"; break;
                    }
            }
            string _vvo = "0";
            string _vvo56 = "1";
            string _vvo52 = "";

            if (listGrp.Contains(Convert.ToInt32(row["grp"])))
                _vvo = "5";

            string value_11 = "0";

            if (dtPromoGoods != null && dtPromoGoods.Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> rowCollect = dtPromoGoods.AsEnumerable()
                    .Where(r => r.Field<int>("id_tovar") == int.Parse(row["id_tovar"].ToString()));
                if (rowCollect.Count() > 0 && !listIDPromoGoods.Contains(int.Parse(row["id_tovar"].ToString())))
                {
                    value_11 = "000000000002";
                    string _tmp = $"{row["id_tovar"]};;000000000001;{rowCollect.First()["SalePrice"]};;;";
                    listPromoGoods.Add(_tmp);
                    listIDPromoGoods.Add(int.Parse(row["id_tovar"].ToString()));
                }
            }
            if (TypeMarking != null && TypeMarking.Rows.Count > 0)
            {
                if (GoodMarking != null && GoodMarking.Rows.Count > 0)
                    GoodMarking.DefaultView.RowFilter = "id_grp1 = " + grp;

                EnumerableRowCollection<DataRow> rowCollect = TypeMarking.AsEnumerable()
                   .Where(r => r.Field<int>("id_grp1") == int.Parse(grp));

                //if (rowGoods.Length > 0)
                if(GoodMarking != null && GoodMarking.DefaultView.Count > 0)
                {
                    if (GoodMarking.AsEnumerable().Any(r => r.Field<int>("id_tovar") == int.Parse(id_tovar)))
                    {
                        _vvo = $"{rowCollect.First()["typeXposCode"]}";
                        _vvo52 = $"{rowCollect.First()["is_CheckMarking"]}";
                    }
                }
                else
                {
                    if (rowCollect.Count() > 0)
                    {
                        _vvo = $"{rowCollect.First()["typeXposCode"]}";
                        _vvo52 = $"{rowCollect.First()["is_CheckMarking"]}";
                    }
                }
                if (_vvo52.Length > 0)
                    _vvo52 = _vvo52.ToLower().Equals("true") ? "1" : "0";
            }

            string pole66 = "";
            if (Convert.ToInt32(id_unit) == 2)
                pole66 = "0";
            else if (Convert.ToInt32(id_unit) == 1)
                pole66 = "2";

            string str = id_tovar + ";" + ean + ";" + name + ";" + name + ";" + price + ";1000;0;" + weight +
                ";0;0;" + value_11 + ";0;1;1;0;" + grp + ";1;0;;" + firm + ";;" +
                ";" + tax + ";;;" + id_post + ";;;;;;;;;;;;;;;;;;;;;;;;;;" + _vvo52 + ";" + kodVVO.Trim() + ";;" + _vvo + ";" + _vvo56 + ";40.0"
                + ";2;;0,0;;;;;0;" + pole66;
            return str;
        }

        private string[] GetItemSetting()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("UserSettings.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode item in xRoot.ChildNodes)
            {
                foreach(XmlNode item1 in item.ChildNodes)
                {
                    if (item1.Attributes.GetNamedItem("key").Value == "weightGood")
                        return item1.Attributes.GetNamedItem("value").Value.Split(',');
                }
            }
            return null;
        }
    }
}