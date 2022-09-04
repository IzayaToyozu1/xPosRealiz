using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using xPosBL.Terminals.Data;
using xPosBL.Terminals.States;
using xPosBL.Terminals.Data.Sending;
using System.Threading;

namespace xPosBL.Terminals.Controls
{
    public class ControlerTerminal
    {
        private CancellationToken _token;

        public DataTable Terminals { get; private set; }
        public IDataGoods DataGoods { get; private set; }
        public TerminalType[] Type { get; private set; }
        public List<Terminal> TerminalWork { get; private set; }
        public SettingXPOS Settings {get; private set;}

        public EventHandler<string> Message;

        public event EventHandler ChangedTerminals;

        public ControlerTerminal(IDataGoods<DataTable> dataGoods, CancellationToken token)
        {
            _token = token;
            DataGoods = dataGoods;
            UpdateTerminals();
            DataTable TT = SQL.GetTerminalType(true);
            Type = new TerminalType[TT.Rows.Count];
            int i = 0;
            foreach(DataRow row in TT.Rows)
            {
                Type[i].Id = Convert.ToInt32(row["id"]);
                Type[i++].Name = row["NameTerminalType"].ToString();
            }
            Settings = new SettingXPOS();
            TerminalWork = new List<Terminal>();
        }

        public void UpdateTerminals()
        {
            IDataGoods<DataTable> d = DataGoods as IDataGoods<DataTable>;
            Terminals = d.GetTerminals();
            Terminals.RowChanged += ChangeSelectionTerminal;
            ChangedTerminals?.Invoke(Terminals, EventArgs.Empty);
            if(TerminalWork != null)
                foreach(DataRow rowTerm in Terminals.Rows)
                {
                    int numberTerm = Convert.ToInt32(rowTerm["Number"]);
                    Terminal term = TerminalWork.LastOrDefault(t => t.Number == numberTerm);
                    if(term != null)
                    {
                        term.IdGU = Convert.ToInt32(rowTerm["id_gu"].ToString());
                    }
                }
            GC.Collect();
        }

        public void UpdateTerminalWorking()
        {
            EnumerableRowCollection<DataRow> terminalsIsSelect = Terminals.AsEnumerable().Where(r => r.Field<bool>("isSelect"));
            foreach (DataRow row in terminalsIsSelect)
            {
                Terminal terminal = TerminalWork.LastOrDefault(t => t.Number == Convert.ToInt32(row["Number"].ToString()));
                if (terminal == null)
                {
                    terminal = new Terminal();
                    TerminalWork.Add(terminal);
                    terminal.Path = "\\\\" + row["IP"] + row["path"];
                    terminal.Number = Convert.ToInt32(row["Number"].ToString());
                    terminal.TypeId = Convert.ToInt32(row["id_TerminalType"].ToString());
                    terminal.IdGU = Convert.ToInt32(row["id_gu"].ToString());
                    terminal.Worker = false;
                    terminal.Setting = Settings;
                    ISending sending = new SendingCatalogGoods(terminal, _token);
                    sending.EventMessage += Message;
                    terminal.State = new StateTerminalNotBusy(terminal, sending, _token);
                    terminal.State.EventMessage += Message;
                }
            }

            if(!terminalsIsSelect.Any())
            {
                TerminalWork.Clear();
            }
            
            foreach (var terminal in TerminalWork.ToArray())
            {
                bool check = true;
                foreach (DataRow rows in terminalsIsSelect)
                {
                    check = terminal.Number == Convert.ToInt32(rows["Number"].ToString());
                    if (check)
                        break;
                }
                if (!check)
                    TerminalWork.Remove(terminal);
            }
        }

        public void ChangeSelectionTerminal(object sender, DataRowChangeEventArgs e)
        {
            DataGoods.SetTerminals(Terminals);
            UpdateTerminalWorking();
        }

        public void SetTerminalFilter(int id)
        { 
            if(id == 0)
                Terminals.DefaultView.RowFilter = "";
            else
            Terminals.DefaultView.RowFilter = "id_TerminalType = " + id.ToString();
        }

        public void ChangedTerminalChoice(int numberTerminal)
        {
            DataRow row;
            foreach(DataRow r in Terminals.Rows)
            {
                if(Convert.ToInt32(r["Number"]) == numberTerminal)
                {
                    row = r;
                    if (Convert.ToBoolean(row["isSelect"]) == true)
                        row["isSelect"] = false;
                    else
                        row["isSelect"] = true;
                    break;
                } 
            }
        }

        public void ChangedAllTerminalChoice()
        {
            bool result = true;
            foreach(DataRow r in Terminals.Rows)
            {
                if (!Convert.ToBoolean(r["isSelect"]))
                {
                    result = true;
                    break;
                }
                result = false;
            }

            foreach(DataRow r in Terminals.Rows)
            {
                r["isSelect"] = result;
            }
        }
    }
}
