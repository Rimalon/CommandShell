using System;

namespace Commands
{
    public class CommandException : Exception
    {
        public override string Message { get; }

        public CommandException(string message)
        {
            Message = message;
        }
    }
}
