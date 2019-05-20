using System.Collections.Generic;

namespace Commands
{
    public abstract class Command
    {
        public List<string> Arguments { get; protected set; }
        public  string TargetPath { get; protected set; }
        protected  bool IsFirstCommand { get; set; }

        public abstract void Execute();
        public abstract bool IsCorrectArgs();

    }
}
