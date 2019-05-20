using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Exit : Command
    {
        public Exit(List<string> arguments)
        {
            Arguments = arguments;
        }
        public override void Execute()
        {
            throw new CommandException("Exit");
        }

        public override bool IsCorrectArgs()
        {
            if (Arguments.Any())
            {
                throw new CommandException("Incorrect pwd command args");
            }
            return true;
        }
    }
}
