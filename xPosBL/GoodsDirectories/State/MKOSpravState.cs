using System.Data;
using System;

namespace xPosBL.GoodsDirectories.State
{
    public class MKOSpravState : ISpravState
    {
        private ISpravState s;
        private SettingsSprav _settings;
        
        public MKOSpravState(SettingsSprav Settings)
        {
            _settings = Settings;
        }

        public DataTable GetListGoods(DataTable goods, int idLust, int idTypeGoods)
        {
            s = new VVOSpravState();
            if (idTypeGoods == 1 && _settings.MKOandGST)
            {
                DataTable result = goods.Clone();
                goods.DefaultView.RowFilter = $"id_goodsUpdate > {idLust} and id_departments == 1";
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
