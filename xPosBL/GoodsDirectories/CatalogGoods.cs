using System;
using System.Data;
using System.Threading.Tasks;
using xPosBL.GoodsDirectories.Command;
using xPosBL.GoodsDirectories.CreateSprav;
using xPosBL.GoodsDirectories.GetGoods;
using xPosBL.GoodsDirectories.State;

namespace xPosBL.GoodsDirectories
{
    public class CatalogGoods
    {
        private readonly object _locking = new object();
        private readonly SpravCommands _spravCommands = new SpravCommands();
        private readonly ISpravState _state;
        private readonly GetGoodsFullFromServer _goodsFull;
        
        public SettingsSprav SettingsSprav { get; set; }

        public event EventHandler<string> EventMessage;

        public EventHandler StartCreateSprav;
        public EventHandler EndCreateSprav;

        public CatalogGoods(string fileNameFullSprav, SettingsSprav settings)
        {
            _spravCommands.AddNewCommand("CreateFull", 
                CreatComSpr(fileNameFullSprav, new CreateFullSpravWorkTerminal()));
            _spravCommands.AddNewCommand("CreateFullVVO", 
                CreatComSpr(fileNameFullSprav + "_VVO", new CreateFullSpravWorkTerminal()));
            _spravCommands.AddNewCommand("CreateFullNotWork", 
                CreatComSpr(fileNameFullSprav, new CreateFullSprav()));
            _spravCommands.AddNewCommand("CreateFullNotWorkVVO", 
                CreatComSpr(fileNameFullSprav + "_VVO", new CreateFullSprav()));
            _goodsFull = new GetGoodsFullFromServer();
            SettingsSprav = settings;
            _state = new GSTSpravState(SettingsSprav);
        }

        private SpravCreateCom CreatComSpr(string fileName, ICreateSprav createSprav)
        {
            createSprav.EventStartCreating += StartCreateSprav;
            createSprav.EventEndCreating += EndCreateSprav;
            return new SpravCreateCom(fileName, createSprav);
        }

        public void CreateFullSprav(bool terminalWork)
        {
            Task.Run(() =>
            {
                StartCreateSprav?.Invoke(new object(), EventArgs.Empty);
                if (terminalWork)
                {
                    _spravCommands.ComExecude("CreateFull", _goodsFull.GetGoods());
                    _spravCommands.ComExecude("CreateFullVVO", _goodsFull.GetGoodsVVO());
                }
                else
                {
                    _spravCommands.ComExecude("CreateFullNotWork", _goodsFull.GetGoods());
                    _spravCommands.ComExecude("CreateFullNotWorkVVO", _goodsFull.GetGoodsVVO());
                }
                EndCreateSprav?.Invoke(new object(),EventArgs.Empty);
            });
        }

        public DataTable GetGoods(DataTable goods, int idTypeGoods, int idLust)
        {
            lock (_locking)
            {
                if (goods == null)
                    return null;
                return _state.GetListGoods(goods, idLust, idTypeGoods);
            }
        }
    }
}
