namespace xPosBL.Terminals.TypeTerminal
{
    public interface ITerminalType
    {
        int Id { get; }
        void GetDataTerminal();
        void SetDataTerminal();
    }
}
