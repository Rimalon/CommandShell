using System.Collections.Generic;
using Commands;

namespace CommandShell
{
    class Instruction
    {
        public InstructionType Type { get; }

        public string VariableName { get; }
        public object VariableValue { get; }
        
        public List<Command> Commands { get; }

        public Instruction(InstructionType type, List<Command> commandsList)
        {
            Type = type;
            Commands = commandsList;
        }

        public Instruction(InstructionType type, string variableName, object variableValue)
        {
            Type = type;
            VariableName = variableName;
            VariableValue = variableValue;
        }

        public Instruction(InstructionType type, string cmdCommand)
        {
            Type = type;
            Commands = new List<Command> {new CmdCommand(cmdCommand)};

        }
    }
}
