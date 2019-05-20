using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class Cat : Command
    {
        public Cat(List<string> arguments, string targetPath, bool isFirst)
        {
            TargetPath = targetPath;
            IsFirstCommand = isFirst;
            Arguments = IsFirstCommand ? arguments : new List<string>();
        }

        public override void Execute()
        {

            try
            {
                File.Copy(Arguments[0], TargetPath);
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
                throw new CommandException("Incorrect cat command args");
            }

            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new CommandException("Incorrect cat command args");
            }

            


            return true;
        }
    }
}
