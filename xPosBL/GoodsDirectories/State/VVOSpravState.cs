using System.Data;

namespace xPosBL.GoodsDirectories.State
{
    public class VVOSpravState : ISpravState
    {
        ISpravState s;
        public DataTable GetListGoods(DataTable goods, int idLust, int idTypeGoods)
        {
            s = new FullSpravState();
            if(idTypeGoods == 4)
            {
                DataTable result = goods.Clone();
                goods.DefaultView.RowFilter = $"id_goodsUpdate > {idLust}";
                foreach (DataRow row in goods.DefaultView)
                {
                    result.Rows.Add(row.ItemArray);
                }
                return result;
            }
            return s.GetListGoods(goods, idLust, idTypeGoods);
        }
    }
}
