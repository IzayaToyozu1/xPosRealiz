using System.Data;
using System;

namespace xPosBL.GoodsDirectories.State
{
    class GSTSpravState : ISpravState
    {
        private SettingsSprav _settings;

        public GSTSpravState(SettingsSprav settings)
        {
            _settings = settings;
        }

        public DataTable GetListGoods(DataTable goods, int idLust, int idTypeGoods)
        {
            ISpravState s = new MKOSpravState(_settings);
            if (idTypeGoods == 2 && _settings.MKOandGST)
            {
                DataTable result = goods.Clone();
                goods.DefaultView.RowFilter = $"id_goodsUpdate > {idLust} and id_departments == 2";
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
