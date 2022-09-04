using System.Collections.Generic;
using System;

namespace xPosBL.GoodsDirectories.Command
{
    public class SpravCommands
    {
        private Dictionary<string, ISpravCommand> _spravCommands;

        public SpravCommands()
        {
            _spravCommands = new Dictionary<string, ISpravCommand>();
        }

        public ISpravCommand GetCommand(string command)
        {
            return _spravCommands[command];
        }

        public void ComExecude(string com)
        {
            _spravCommands[com].Execude();
        }

        public T ComExecude<T>(string com)
        {
            ISpravCommand command = _spravCommands[com];
            return command.Execude<T>();
        }

        public void ComExecude(string com, object obj)
        {
            _spravCommands[com].Execude(obj);
        }

        public T ComExecude<T>(string com, object obj)
        {
            ISpravCommand command = _spravCommands[com];
            return command.Execude<T>(obj);
        }

        public void AddNewCommand(string nameCom, ISpravCommand command)
        {
            _spravCommands.Add(nameCom, command);
        }

        public void AddNewCommand(string[] nameComs, ISpravCommand[] commands)
        {
            if (nameComs.Length == commands.Length)
            {
                for (int i = 0; i < nameComs.Length; i++)
                {
                    _spravCommands.Add(nameComs[i], commands[i]);
                }
            }
            else
                throw new Exception("Количество строк \"nameComs\" должно совподать с количеством \"commands\"");
        }
    }
}
