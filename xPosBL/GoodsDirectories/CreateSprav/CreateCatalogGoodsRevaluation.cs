using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace xPosBL.GoodsDirectories.CreateSprav
{
    public class CreateCatalogGoodsRevaluation : ICreateSprav<StringBuilder>, ICreateSprav
    {
        public LineFormation LineForm { get; set; }
        public CancellationToken Token { get; set; }

        public event EventHandler EventStartCreating;
        public event EventHandler EventEndCreating;
        public event EventHandler<string> EventErrorCreating;

        public StringBuilder Create(string fileName, DataTable goods)
        {
            LineForm = new LineFormation();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion

            EventStartCreating?.Invoke(this, EventArgs.Empty);
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion

            StringBuilder strAIn = new StringBuilder();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion

            try
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                DataTable deps = SQL.getListDeps();
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                DataTable dtGrp = SQL.getListGrp();
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                var grop = from row in goods.AsEnumerable()
                           group row by row.Field<int>("grp") into grp
                           select new
                           {
                               grpID = grp.Key,
                           };
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion
                var mark = from row in goods.AsEnumerable()
                           //where row.Field<int>("IsCatPromTovar") != -1
                           group row by row.Field<int>("id_post") into post
                           select new
                           {
                               idPost = post.Key,
                           };

                DataTable dtPosts = new DataTable();
                //dtPosts.Columns.Add(new DataColumn("name", typeof(string)));
                //dtPosts.Columns.Add(new DataColumn("numPost", typeof(string)));
                //dtPosts.Columns.Add(new DataColumn("inn", typeof(int)));
                //dtPosts.Columns.Add(new DataColumn("id", typeof(int)));
                if (mark.Any())
                {
                    IEnumerable<DataRow> p = new DataTable().AsEnumerable();
                    foreach (var item in mark)
                    {
                        var post = SQL.GetPosts(item.idPost);
                        p = p.AsEnumerable().Union(post.AsEnumerable());
                    }

                    if (p.Any())
                        dtPosts = p.CopyToDataTable();
                    else
                        dtPosts = null;
                }

                strAIn.Append("##@@&&\r\n#\r\n");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                strAIn.Append("$$$ADDAGENTDETAILS\r\n");
                if (dtPosts != null && dtPosts.Rows.Count > 0)
                    foreach (DataRow row in dtPosts.Rows)
                    {
                        string post = LineForm.StringInserPost(row) + "\r\n";
                        strAIn.Append(post);
                    }

                strAIn.Append("\r\n$$$DELETEWARESBYWARECODE\r\n");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                goods.DefaultView.Sort = "grp ASC, name ASC";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                goods.DefaultView.RowFilter = "";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                goods.DefaultView.RowFilter = "";
                //goods.DefaultView.RowFilter = "ntypetovar = 1";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                foreach (DataRowView row in goods.DefaultView)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    strAIn.Append(row["id_tovar"] + "\r\n");

                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                //goods.DefaultView.RowFilter = "IsCatPromTovar <> -1 and ntypetovar <> 1 ";
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion

                //foreach (DataRowView row in goods.DefaultView)
                //{
                //    #region проверка отмены
                //    AdditionalFunctions.ThrowExceptionToken(Token);
                //    #endregion

                //    strAIn.Append(row["id_tovar"] + "\r\n");
                //}
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion

                strAIn.Append("\r\n$$$ADDQUANTITY\r\n");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion


                foreach (DataRow rdep in deps.Rows)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    strAIn.Append(LineForm.StringInserGRP(rdep ,"id", "name") + "\r\n");
                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion


                foreach (var r in grop)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    DataRow[] row = dtGrp.Select("id = " + r.grpID);
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    if (row.Count() > 0)
                    {
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(Token);
                        #endregion

                        int idotdel = Convert.ToInt32(row[0]["id_otdel"].ToString());
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(Token);
                        #endregion

                        if (idotdel == 6)
                        {
                            continue;
                        }
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(Token);
                        #endregion

                        DataRow[] rDeps = deps.Select("id = " + idotdel);
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(Token);
                        #endregion

                        strAIn.Append(LineForm.StringInserGRP(row[0], "id", "cname", rDeps[0][0].ToString()) + "\r\n");
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(Token);
                        #endregion
                    }
                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                goods.DefaultView.Sort = "grp ASC, name ASC, id_goodsUpdate asc";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                goods.DefaultView.RowFilter = "ntypetovar <> 1 and grp <> 610 and grp <> 612";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion


                foreach (DataRowView row in goods.DefaultView)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    strAIn.Append(LineForm.StringInserTovar(row) + "\r\n");
                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                //strAIn.Append("$$$DELETEWARESBYWARECODE\r\n");
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion


                //goods.DefaultView.Sort = "grp ASC, name ASC";
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion

                //goods.DefaultView.RowFilter = "ntypetovar = 1";
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion


                //foreach (DataRowView row in goods.DefaultView)
                //{
                //    #region проверка отмены
                //    AdditionalFunctions.ThrowExceptionToken(Token);
                //    #endregion

                //    strAIn.Append(row["id_tovar"] + "\r\n");
                //}
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion



                if (LineForm.listPromoGoods.Count > 0)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    strAIn.Append("\n\n\n\n$$$ADDASPECTREMAINS\r\n");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion

                    foreach (string str in LineForm.listPromoGoods)
                    {
                        strAIn.Append(str + "\r\n");
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(Token);
                        #endregion
                    }
                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                EventEndCreating?.Invoke(this, EventArgs.Empty);
            }
            catch(OperationCanceledException)
            {
                EventErrorCreating?.Invoke(this, "Отмена операции");
                throw;
            }

            catch (Exception)
            {
                EventErrorCreating?.Invoke(this, "Ошибка создания файла.");
                throw;
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion

            goods = null;

            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion
            GC.Collect();

            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion
            return strAIn;
        }

        void ICreateSprav.Create(string fileName, DataTable goods)
        {
        }
    }
}
