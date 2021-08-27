namespace HorstRunner
{
    public class CommandBase
    {
        public string CommandId { get; }
        public string CommandDescription { get; }
        public string CommandFormat { get; }

        public CommandBase(string commandId, string commandDescription, string commandFormat)
        {
            this.CommandId = commandId;
            this.CommandDescription = commandDescription;
            this.CommandFormat = commandFormat;
        }
    }
}