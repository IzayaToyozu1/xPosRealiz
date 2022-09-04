using System.Data;

namespace xPosBL.GoodsDirectories.GetGoods
{
    public class GetGoodsFullFromServer : GetGoodsFromServer
    {
        public GetGoodsFullFromServer()
        {
        }

        public override DataTable GetGoods()
        {
            return SQL.getListTovar();
        }

        public override DataTable GetGoodsVVO()
        {
            return SQL.getListTovarVVO();
        }
    }
}
