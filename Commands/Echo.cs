using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class Echo : Command
    {
        public Echo(List<string> arguments, string targetPath,bool isFirst)
        {
            TargetPath = targetPath;
            IsFirstCommand = isFirst;
            Arguments = IsFirstCommand ? arguments : new List<string>();
        }

        public override void Execute()
        {
            using (var writer = File.CreateText(TargetPath))
            {
                foreach (var arg in Arguments)
                {
                    writer.WriteLine(arg);
                }
            }
        }

        public override bool IsCorrectArgs()
        {
            if (!IsFirstCommand && Arguments.Any())
            {
                throw new CommandException("Incorrect echo command args");
            }

            return true;
        }

    }
}
