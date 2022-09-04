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
        internal DataTable dtPromoGoods;
        internal DataTable GoodMarking;
        internal List<int> listGrp;
        internal List<int> listIDPromoGoods;
        internal List<string> listPromoGoods;
        internal DataTable TypeMarking;

        public LineFormation()
        {
            listGrp = new List<int>();
            listPromoGoods = new List<string>();
            listIDPromoGoods = new List<int>();
            dtPromoGoods = SQL.getCatalogPromotionalTovars();

            TypeMarking = Config.hCntMain.getGrp1VsTypeMarking().Result;
            GoodMarking = Config.hCntMain.getGoodvsMarking().Result;
            if (GoodMarking != null && GoodMarking.Rows.Count > 0)
            {
                var rowCollect = GoodMarking.AsEnumerable().Where(r => r.Field<object>("isInclude") != null);
                if (rowCollect.Any())
                    GoodMarking = rowCollect.CopyToDataTable();
                else
                    GoodMarking.Clear();
            }
        }

        internal string StringInserGRP(DataRow rDep, string colId, string colName)
        {
            return StringInserGRP(rDep, colId, colName, "");
        }

        internal string StringInserGRP(DataRow rDep, string colId, string colName, string colIdParent)
        {
            var id = rDep[colId].ToString();
            var name = rDep[colName].ToString();
            var str = id + ";;" + name + ";" + name + ";;;;;;;;;;;;" + colIdParent + ";0";
            return str;
        }

        internal string StringInserPost(DataRow rPost)
        {
            var namePost = rPost["cname"].ToString();
            var numberPhone = rPost["Phone"].ToString().Trim('-').Trim('(', ')');
            var inn = rPost["inn"].ToString();
            var idPost = $"100{rPost["id"]}";
            return $"{idPost};{namePost};6;;;;;;;;{numberPhone};{namePost};{inn}";
        }

        internal string StringInserTovar(DataRowView row)
        {
            var id_tovar = row["id_tovar"].ToString();
            var ean = row["ean"].ToString().Trim();
            var name = row["name"].ToString().Replace(";", " ");
            var price = row["price"].ToString();
            var grp = row["grp"].ToString();
            var kodVVO = row["kodVVO"].ToString();
            var firm = row["firm"].ToString();
            var id_post = row["id_post"].ToString();
            var weight = row["ean"].ToString().Trim().Length == 4 ? "1" : "0";
            var id_unit = row["id_unit"].ToString();

            var good = GetItemSetting();
            if (good.Contains(row["ean"].ToString().Trim()))
                weight = "1";

            var tax = string.Empty;
            switch (row["tax"].ToString())
            {
                case "18":
                    tax = "1";
                    break;
                case "20":
                    tax = "1";
                    break;
                case "10":
                    tax = "2";
                    break;
                default:
                {
                    if (!File.Exists(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
                        using (var fs = File.Create(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" +
                                                    DateTime.Now.Year + ".txt"))
                        {
                            fs.Close();
                        }

                    using (var sw = new StreamWriter(
                               DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt", true))
                    {
                        sw.WriteLine("------------------error");
                        sw.WriteLine("id_tovar = " + id_tovar + ";ean = " + ean + ";name = " + name + ";price = " +
                                     price + ";tax = " + row["tax"]);
                        sw.WriteLine("------------------");
                        sw.Close();
                    }

                    tax = "3";
                    break;
                }
            }

            var _vvo = "0";
            var _vvo56 = "1";
            var _vvo52 = "";
            var field59 = "";
            var value_11 = "0";

            field59 = $"100{id_post}";

            if (dtPromoGoods != null && dtPromoGoods.Rows.Count > 0)
            {
                var rowCollect = dtPromoGoods.AsEnumerable()
                    .Where(r => r.Field<int>("id_tovar") == int.Parse(row["id_tovar"].ToString()));
                if (rowCollect.Count() > 0 && !listIDPromoGoods.Contains(int.Parse(row["id_tovar"].ToString())))
                {
                    value_11 = "000000000002";
                    var _tmp = $"{row["id_tovar"]};;000000000001;{rowCollect.First()["SalePrice"]};;;";
                    listPromoGoods.Add(_tmp);
                    listIDPromoGoods.Add(int.Parse(row["id_tovar"].ToString()));
                }
            }

            if (TypeMarking != null && TypeMarking.Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> rowGoods = null;
                if (GoodMarking != null && GoodMarking.Rows.Count > 0)
                    rowGoods = GoodMarking.AsEnumerable().Where(r => r.Field<int>("id_grp1") == int.Parse(grp));

                var rowCollect = TypeMarking.AsEnumerable()
                    .Where(r => r.Field<int>("id_grp1") == int.Parse(grp));

                if (rowCollect.Any())
                {
                    _vvo = $"{rowCollect.First()["typeXposCode"]}";
                    _vvo52 = $"{rowCollect.First()["is_CheckMarking"]}";
                }
                else if ((rowGoods.AsEnumerable() ?? Array.Empty<DataRow>()).Any(r => r.Field<int>("id_tovar") == int.Parse(id_tovar)))
                {
                    _vvo = $"{rowGoods.First()["typeXposCode"]}";
                    _vvo52 = $"{rowGoods.First()["is_CheckMarking"]}";
                }

                //else
                //{
                //    if (rowCollect.Count() > 0)
                //    {
                //        _vvo = $"{rowCollect.First()["typeXposCode"]}";
                //        _vvo52 = $"{rowCollect.First()["is_CheckMarking"]}";
                //    }
                //}
                if (_vvo52.Length > 0)
                    _vvo52 = _vvo52.ToLower().Equals("true") ? "1" : "0";
            }

            var field66 = "";
            if (Convert.ToInt32(id_unit) == 2)
                field66 = "0";
            else if (Convert.ToInt32(id_unit) == 1)
                field66 = "2";

            var str = $"{id_tovar};{ean};{name};{name};{price};1000;0;{weight};0;0;{value_11}" +
                      $";0;1;1;0;{grp};1;;;{firm};;;{tax};;;{id_post};;;;;;;;;;;;;;;;;;;;;;;;;;" +
                      $"{_vvo52};{kodVVO.Trim()};;{_vvo};{_vvo56};40.0;2;{field59};0,0;;;;;0;{field66}";
            return str;
        }

        private string[] GetItemSetting()
        {
            var xDoc = new XmlDocument();
            xDoc.Load("UserSettings.xml");
            var xRoot = xDoc.DocumentElement;
            foreach (XmlNode item in xRoot.ChildNodes)
            foreach (XmlNode item1 in item.ChildNodes)
                if (item1.Attributes.GetNamedItem("key").Value == "weightGood")
                    return item1.Attributes.GetNamedItem("value").Value.Split(',');
            return null;
        }
    }

    #region Код в случие если перестанет формироваться фулл справочник

    /*
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
            if (GoodMarking != null && GoodMarking.Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> rowCollect = GoodMarking.AsEnumerable().Where(r => r.Field<object>("isInclude") != null);
                if (rowCollect.Count() > 0)
                    GoodMarking = rowCollect.CopyToDataTable();
                else
                    GoodMarking.Clear();
            }
        }

        internal string StringInserGRP(DataRow rDep, string colId, string colName)
        {
            return StringInserGRP(rDep, colId, colName, "");
        }

        internal string StringInserGRP(DataRow rDep, string colId, string colName, string colIdParent)
        {
            string id = rDep[colId].ToString();
            string name = rDep[colName].ToString();
            string str = id + ";;" + name + ";" + name + ";;;;;;;;;;;;" + colIdParent + ";0";
            return str;
        }

        internal void kek()
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
                EnumerableRowCollection<DataRow> rowGoods = null;
                if (GoodMarking != null && GoodMarking.Rows.Count > 0)
                    rowGoods = GoodMarking.AsEnumerable().Where(r => r.Field<int>("id_grp1") == int.Parse(grp));

                EnumerableRowCollection<DataRow> rowCollect = TypeMarking.AsEnumerable()
                   .Where(r => r.Field<int>("id_grp1") == int.Parse(grp));

                if (rowGoods.Count() > 0)
                {
                    bool isInclude = (bool)rowGoods.First()["isInclude"];

                    if (isInclude)
                    {
                        if (rowGoods.AsEnumerable().Where(r => r.Field<int>("id_tovar") == int.Parse(id_tovar)).Count() > 0)
                        {
                            _vvo = $"{rowCollect.First()["typeXposCode"]}";
                            _vvo52 = $"{rowCollect.First()["is_CheckMarking"]}";
                        }
                    }
                    else
                    {
                        if (rowGoods.AsEnumerable().Where(r => r.Field<int>("id_tovar") == int.Parse(id_tovar)).Count() == 0)
                        {
                            _vvo = $"{rowCollect.First()["typeXposCode"]}";
                            _vvo52 = $"{rowCollect.First()["is_CheckMarking"]}";
                        }
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
                ";0;0;" + value_11 + ";0;1;1;0;" + grp + ";1;;;" + firm + ";;" +
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
    }*/

    #endregion
}