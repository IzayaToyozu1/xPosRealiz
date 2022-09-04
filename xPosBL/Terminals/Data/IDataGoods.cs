using System;
namespace xPosBL.Terminals.Data
{
    public interface IDataGoods
    {
        object GetTerminals();
        void SetTerminals(object terminals);
    }

    public interface IDataGoods<T>: IDataGoods
    {
        new T GetTerminals();
        void SetTerminals(T terminals);
    }
}
