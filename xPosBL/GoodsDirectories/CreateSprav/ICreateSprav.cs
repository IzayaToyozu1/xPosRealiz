using System;
using System.Data;

namespace xPosBL.GoodsDirectories.CreateSprav
{
    public enum EnumGroupGoods
    {
        MKO = 1,
        GST = 2,
        VVO = 6,
    }

    public interface ICreateSprav
    {
        event EventHandler EventStartCreating;
        event EventHandler EventEndCreating;
        event EventHandler<string> EventErrorCreating;

        LineFormation LineForm { get; set; }

        void Create(string fileName, DataTable goods);
    }

    public interface ICreateSprav<T>: ICreateSprav
    {
        new T Create(string fileName, DataTable goods);
    }
}
