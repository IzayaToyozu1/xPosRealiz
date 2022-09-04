using xPosBL.GoodsDirectories.CreateSprav;
using System.Data;

namespace xPosBL.GoodsDirectories.Command
{
    public class SpravCreateCom : ISpravCommand
    {
        public string FileName { get; set; } = "";
        public DataTable Goods { get; set; }
        public ICreateSprav CreateSprav {get; set;}

        public SpravCreateCom(string fileName, ICreateSprav createSprav)
        {
            FileName = fileName;
            CreateSprav = createSprav;
        }

        public void Execude()
        {
            if(Goods != null)
                CreateSprav.Create(FileName, Goods) ;
        }

        public T Execude<T>()
        {
            ICreateSprav<T> creating = CreateSprav as ICreateSprav<T>; //TODO Првоерка на Null
            return creating.Create(FileName, Goods);
        }

        public void Execude(object obj)
        {
            DataTable goods = obj as DataTable;
            CreateSprav.Create(FileName, goods);
        }

        public T Execude<T>(object obj)
        {
            DataTable goods = obj as DataTable;
            ICreateSprav<T> creating = CreateSprav as ICreateSprav<T>;
            return creating.Create(FileName, goods);
        }
    }
}
