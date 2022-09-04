namespace xPosBL.Terminals.Data
{
    public interface ISaveLoad
    {
        void Save<T>(T obj, string path) where T : class;
        void Save<T>(T obj)where T: class;
        T Load<T>(string path) where T : class;
        T Load<T>()where T: class;
    }
}
