using System;

namespace CommandShell
{
    class VariableDefException : Exception
    {
        public override string Message { get; }

        public VariableDefException(string message)
        {
            Message = message;
        }
    }
}
