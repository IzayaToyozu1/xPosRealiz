using System.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace xPosBL.GoodsDirectories.GetGoods
{
    public class GetGoodsFromServer
    { 
        public Config Confing { get; set; }

        public virtual DataTable GetGoods()
        {
            return GetGoods(0);
        }

        public virtual DataTable GetGoods(int lastIdGoods)
        {
            DataTable GoodsDbase1 = Config.hCntMain.getListGoodsDbase1(false);
            DataTable GoodUpdates = Config.hCntMainKassRealiz.getListGoodsKassRealiz(lastIdGoods);
            DataTable dtGoodsToFile = CreateDataTableTovar();
            var query = GetTableTovar(dtGoodsToFile, GoodsDbase1, GoodUpdates);
            if (query.Count() == 0)
                return null;
            dtGoodsToFile = query.CopyToDataTable();
            return dtGoodsToFile;
        }

        public virtual DataTable GetGoodsVVO()
        {
            return GetGoodsVVO(0);
        }

        public virtual DataTable GetGoodsVVO(int lastIdGoods)
        {
            DataTable GoodsVvoDbase1 = Config.hCntSecond.getListGoodsDbase1(true);
            DataTable GoodUpdates = Config.hCntMainKassRealiz.getListGoodsKassRealiz(lastIdGoods);
            DataTable dtGoodsVVOToFile = CreateDataTableTovar();
            var queryVVO = GetTableTovar(dtGoodsVVOToFile, GoodsVvoDbase1, GoodUpdates);
            if (queryVVO.Count() == 0)
                return null;
            dtGoodsVVOToFile = queryVVO.CopyToDataTable();
            return dtGoodsVVOToFile;
        }

        public int GetLastIdGoodTerminal()
        {
            return Convert.ToInt32(Config.hCntMainKassRealiz.GetLastIdGoodTerminal().Rows[0][0]);
        }

        private IEnumerable<DataRow> GetTableTovar(DataTable dt, DataTable GoodsDbase1, DataTable GooodUpdates)
        {
            var query = from g in GoodsDbase1.AsEnumerable()
                        join k in GooodUpdates.AsEnumerable() on new { Q = g.Field<string>("ean").Trim() } equals new { Q = k.Field<string>("ean").Trim() }
                        select dt.LoadDataRow(new object[]
                                                       {

                                                               g.Field<string>("ean"),
                                                               k.Field<DateTime>("r_time"),
                                                               k.Field<string>("name"),
                                                               k.Field<decimal>("price"),
                                                               k.Field<int>("grp"),
                                                               k.Field<int>("tax"),
                                                               g.Field<string>("id_tovar"),
                                                               g.Field<string>("kodVVO"),
                                                               g.Field<int>("ntypetovar"),
                                                               g.Field<int>("id_unit"),
                                                               g.Field<string>("firm"),
                                                               g.Field<int?>("id_post"),
                                                               k.Field<int>("id_departments"),
                                                               k.Field<int>("id")
                                                               ,g.Field<int>("IsCatPromTovar")
                                                       }, false);
            return query;
        }

        private DataTable CreateDataTableTovar()
        {
            DataTable dataTableGoodsUpdats = new DataTable();
            dataTableGoodsUpdats.Columns.Add("ean", typeof(string));
            dataTableGoodsUpdats.Columns.Add("r_time", typeof(DateTime));
            dataTableGoodsUpdats.Columns.Add("name", typeof(string));
            dataTableGoodsUpdats.Columns.Add("price", typeof(decimal));
            dataTableGoodsUpdats.Columns.Add("grp", typeof(int));
            dataTableGoodsUpdats.Columns.Add("tax", typeof(int));
            dataTableGoodsUpdats.Columns.Add("id_tovar", typeof(string));
            dataTableGoodsUpdats.Columns.Add("kodVVO", typeof(string));
            dataTableGoodsUpdats.Columns.Add("ntypetovar", typeof(int));
            dataTableGoodsUpdats.Columns.Add("id_unit", typeof(int));
            dataTableGoodsUpdats.Columns.Add("firm", typeof(string));
            dataTableGoodsUpdats.Columns.Add("id_post", typeof(int));
            dataTableGoodsUpdats.Columns.Add("id_departments", typeof(int));
            dataTableGoodsUpdats.Columns.Add("id_goodsUpdate", typeof(int));
            dataTableGoodsUpdats.Columns.Add("IsCatPromTovar", typeof(int));
            dataTableGoodsUpdats.AcceptChanges();
            return dataTableGoodsUpdats;
        }
    }
}
