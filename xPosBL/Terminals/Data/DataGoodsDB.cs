using System.Linq;
using System.Data;

namespace xPosBL.Terminals.Data
{
    public class DataGoodsDB : IDataGoods<DataTable>
    {
        public ISaveLoad Load { get; set; }
        
        public DataGoodsDB(ISaveLoad load)
        {
            Load = load;
        }

        object IDataGoods.GetTerminals()
        {
            return new object();
        }

        public DataTable GetTerminals()
        {
            DataTable tb = SQL.GetSpravTerminal();
            tb.DefaultView.Sort = "Number asc";
            return GetDataColamn(tb);
        }

        private DataTable GetDataColamn(DataTable tb)
        {
            int[] idTerminal = Load.Load<int[]>();
            if (tb != null && tb.Rows.Count > 0 && idTerminal != null && idTerminal.Length != 0)
            {
                EnumerableRowCollection<DataRow> rowCollect = tb.AsEnumerable().Where(r => idTerminal.Contains(r.Field<int>("id")));
                foreach (DataRow row in rowCollect)
                    row["isSelect"] = true;
                tb.AcceptChanges();
            }
            return tb;
        }

        public void SetTerminals(object terminals)
        {
            DataTable tb = terminals as DataTable;
            if(tb != null)
                SetTerminals(tb);
        }

        public void SetTerminals(DataTable terminals)
        {
            DataTable tb = terminals;
            var term = tb.DefaultView.ToTable().AsEnumerable().Where(r => r.Field<bool>("isSelect")).Select(s => new { id = s.Field<int>("id") }).ToArray();
            int[] idTerminal = new int[term.Length];
            int i = 0;
            foreach(var item in term)
            {
                idTerminal[i] = item.id;
                ++i;
            }
            Load.Save<int[]>(idTerminal);
        }
    }
}
