using xPosBL.Terminals.States;

namespace xPosBL.Terminals
{
    public struct TerminalType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Terminal
    {
        public SettingXPOS Setting { get; set; }
        public IStateTerminal State { get; set; }
        public int Number { get; set; }
        public int IdGU { get; set; }
        public bool Worker { get; set; }
        public string Path { get; set; }
        public int TypeId { get; set; }
        public int HashFile { get; set; }
        public int LastIdGU { get; set; }
    }
}
