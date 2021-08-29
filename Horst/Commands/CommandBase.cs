namespace Horst.Commands
{
    public class CommandBase
    {
        public string CommandId { get; }
        public string CommandDescription { get; }
        public string CommandFormat { get; }

        protected CommandBase(string commandId, string commandDescription, string commandFormat)
        {
            this.CommandId = commandId;
            this.CommandDescription = commandDescription;
            this.CommandFormat = commandFormat;
        }
    }
}