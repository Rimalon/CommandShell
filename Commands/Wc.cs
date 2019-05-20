using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class Wc : Command
    {
       public Wc(List<string> arguments, string targetPath, bool isFirst)
        {
            TargetPath = targetPath;
            IsFirstCommand = isFirst;
            Arguments = IsFirstCommand ? arguments : new List<string>();
        }

        public override void Execute()
        {
            try
            {
                using (var writer = File.CreateText(TargetPath))
                {
                    writer.WriteLine("In {0} {1} lines", Arguments[0], File.ReadAllLines(Arguments[0]).Length);
                    writer.WriteLine("In {0} {1} words", Arguments[0], File.ReadAllText(Arguments[0]).Length);
                    writer.WriteLine("In {0} {1} bytes", Arguments[0], File.ReadAllBytes(Arguments[0]).Length);
                }
            }
            catch (Exception)
            {
                throw new CommandException("Incorrect file path");
            }
        }

        public override bool IsCorrectArgs()
        {
            if (!IsFirstCommand && Arguments.Any())
            {
                throw new CommandException("Incorrect wc command args");
            }
            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new CommandException("Incorrect wc command args");
            }
            
            return true;
        }
    }
}
