using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class Pwd : Command
    {
        public Pwd(List<string> arguments, string targetPath, bool isFirst)
        {
            TargetPath = targetPath;
            Arguments = arguments;
            IsFirstCommand = isFirst;
        }

        public override void Execute()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            using (var writer = File.CreateText(TargetPath))
            {
                writer.WriteLine(currentDirectory);
                foreach (var fileName in Directory.EnumerateFileSystemEntries(currentDirectory))
                {
                    writer.WriteLine(fileName);
                }
            }
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
