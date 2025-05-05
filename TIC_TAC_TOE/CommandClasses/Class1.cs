namespace CommandClasses
{
    public enum CommandType
    {
        WAIT, 
        START,
        RESTART,
        MOVE,
        CLOSE,
        EXIT
    }
    public struct CellCoord
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public override string ToString()
        {
            return $"{RowIndex}:{ColumnIndex}";
        }
    }
    public class ClientCommand
    {
        public CommandType Type { get; set; }
        public string NickName { get; set; }
        public CellCoord MoveCoord { get; set; }

        public ClientCommand(CommandType type, string nick)
        {
            this.Type = type ;
            this.NickName = nick ;
        }
    }
    public class ServerCommand
    {
        public CommandType Type { get; set; }
        public bool IsX { get; set; }
        public string OpponentName { get; set; }
        public CellCoord MoveCoord { get; set; }
        public ServerCommand(CommandType type)
        {
            this.Type = type ;
        }
    }
}