using System.Data;
using System;

namespace xPosBL.GoodsDirectories.State
{
    public class FullSpravState : ISpravState
    {
        public DataTable GetListGoods(DataTable goods, int idLust, int idTypeGoods)
        {
            if (idLust < 0)
            {
                return null;
            }
            DataTable result = goods.Clone();
            goods.DefaultView.RowFilter = "id_goodsUpdate > " + idLust;
            foreach(DataRowView row in goods.DefaultView)
                result.Rows.Add(row.Row.ItemArray);
            return result;
        }
    }
}
