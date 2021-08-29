using System;

namespace Horst.Commands
{
    public class Command: CommandBase
    {
        private readonly Action _command;
        public Command(string id, string description, string format, Action command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke()
        {
            _command.Invoke();
        }
    }
    
    public class Command<T1>: CommandBase
    {
        private readonly Action<T1> _command;
        public Command(string id, string description, string format, Action<T1> command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke(T1 value)
        {
            _command.Invoke(value);
        }
    }
    
    public class Command<T1, T2>: CommandBase
    {
        private readonly Action<T1, T2> _command;
        public Command(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke(T1 value1, T2 value2)
        {
            _command.Invoke(value1, value2);
        }
    }
}