using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace xPosBL.GoodsDirectories.CreateSprav
{
    class CreateCatalogGoodsRevaluationVVO: ICreateSprav<StringBuilder>
    {
        public LineFormation LineForm { get; set; }
        public CancellationToken Token { get; set; }

        public event EventHandler EventStartCreating;
        public event EventHandler EventEndCreating;
        public event EventHandler<string> EventErrorCreating;

        public StringBuilder Create(string fileName, DataTable goods)
        {
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(Token);
            #endregion
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
                DataTable dtDeps = SQL.getListDeps(); 
                DataRow newRow = dtDeps.NewRow();
                newRow["id"] = 6;
                newRow["name"] = "ВВО";
                dtDeps.Rows.Add(newRow);

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
                strAIn.Append("##@@&&\r\n#\r\n$$$DELETEWARESBYWARECODE\r\n");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion
                goods.DefaultView.Sort = "grp ASC, name ASC";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion
                foreach (DataRowView row in goods.DefaultView)
                {
                    strAIn.Append(row["id_tovar"] + "\r\n");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion
                }
                //goods.DefaultView.RowFilter = "ntypetovar <> 1";
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion
                //foreach (DataRowView row in goods.DefaultView)
                //{
                //    strAIn.Append(row["id_tovar"] + "\r\n");
                //    #region проверка отмены
                //    AdditionalFunctions.ThrowExceptionToken(Token);
                //    #endregion
                //}
                //DataTable PromTovars = SQL.GetPromTovar();
                //#region проверка отмены
                //AdditionalFunctions.ThrowExceptionToken(Token);
                //#endregion

                //foreach (DataRow row in PromTovars.DefaultView)
                //{
                //    strAIn.Append(row["id_tovar"] + "\r\n");
                //    #region проверка отмены
                //    AdditionalFunctions.ThrowExceptionToken(Token);
                //    #endregion
                //}
                strAIn.Append("\n$$$ADDQUANTITY\r\n");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                foreach (DataRow rdep in dtDeps.Rows)
                {
                    strAIn.Append(LineForm.StringInserGRP(rdep, "id", "name") + "\r\n");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion
                }

                foreach (var r in grop)
                {
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

                        if (idotdel != 6)
                        {
                            #region проверка отмены
                            AdditionalFunctions.ThrowExceptionToken(Token);
                            #endregion
                            DataRow[] rDeps = dtDeps.Select("id = " + idotdel);
                            #region проверка отмены
                            AdditionalFunctions.ThrowExceptionToken(Token);
                            #endregion
                            strAIn.Append(LineForm.StringInserGRP(row[0], "id", "cname", rDeps[0][0].ToString()) + "\r\n");
                        }
                    }
                }

                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion
                goods.DefaultView.Sort = "grp ASC, name ASC, id_goodsUpdate asc";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion
                goods.DefaultView.RowFilter = "ntypetovar <> 1";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion

                foreach (DataRowView row in goods.DefaultView)
                {
                    strAIn.Append(LineForm.StringInserTovar(row) + "\r\n");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(Token);
                    #endregion
                }
                
                EventEndCreating?.Invoke(this, EventArgs.Empty);
            }
            catch (OperationCanceledException)
            {
                EventErrorCreating?.Invoke(this, "Отмена операции");
                strAIn = null;
                return null;
            }
            catch (Exception e)
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(Token);
                #endregion
                EventErrorCreating?.Invoke(this, e.Message);
                strAIn = null;
                return null;
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
