using System;

namespace HorstRunner
{
    public class Command: CommandBase
    {
        private Action _command;
        public Command(string id, string description, string format, Action command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke()
        {
            _command.Invoke();
        }
    }
}