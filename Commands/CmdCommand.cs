using System;
using System.Collections.Generic;

namespace Commands
{
    public class CmdCommand : Command
    {
        public CmdCommand(string argument)
        {
            Arguments = new List<string> {argument};
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override bool IsCorrectArgs()
        {
            throw new NotImplementedException();
        }
    }
}
