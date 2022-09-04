namespace xPosBL.GoodsDirectories.Command
{
    public interface ISpravCommand
    {
        void Execude();
        void Execude(object obj);
        T Execude<T>();
        T Execude<T>(object obj);
    }
}