using System.Data;

namespace xPosBL.GoodsDirectories.State
{
    public interface ISpravState
    {
        DataTable GetListGoods(DataTable goods, int idLust, int idTypeGoods);
    }
}
