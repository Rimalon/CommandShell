using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Commands;

namespace CommandShell
{
    static class Parser
    {
        public static Instruction Parse(string inStr)
        {
            if (inStr.First() == '$')
            {
                try
                {
                    return GetVariableDefInstrucions(inStr);
                }
                catch (VariableDefException e)
                {
                    throw e;
                }
            }

            try
            {
                return GetCommandInstructions(inStr);
            }
            catch (CommandException)
            {
                return new Instruction(InstructionType.CmdCommand,
                    new List<Command>
                    {
                        new CmdCommand(inStr)
                    });
            }
        }


        private static Instruction GetVariableDefInstrucions(string inStr)
        {
            StringBuilder builder = new StringBuilder(inStr);
            if (!IsCorrectDefinitionSyntax(inStr))
            {
                throw new VariableDefException("Incorrect definition syntax");
            }

            char[] charArrName = new char[inStr.IndexOf('=') - 1];
            builder.CopyTo(1, charArrName, 0, inStr.IndexOf('=') - 1);

            char[] charArrVal = new char[inStr.Length - inStr.IndexOf('=') - 1];
            builder.CopyTo(inStr.IndexOf('=') + 1, charArrVal, 0, inStr.Length - inStr.IndexOf('=') - 1);

            string strVal = new string(charArrVal);
            string strName = new string(charArrName);

            strName = strName.TrimEnd(' ');
            strVal = strVal.Trim();

            if (!IsCorrectVariableName(strName))
            {
                throw new VariableDefException("Incorrect variable name");
            }

            if (Int32.TryParse(strVal, out int intResult))
            {
                return new Instruction(InstructionType.VariableDef, strName, intResult);
            }

            if (Double.TryParse(strVal, out double doubleResult))
            {
                return new Instruction(InstructionType.VariableDef, strName, doubleResult);
            }

            return new Instruction(InstructionType.VariableDef, strName, strVal);
        }

        private static bool IsCorrectDefinitionSyntax(string inStr) =>
            inStr.Contains('=') && (inStr.LastIndexOf('=') == inStr.IndexOf('=')) &&
            inStr.IndexOf('=') != inStr.Length - 1;

        private static bool IsCorrectVariableName(string name) =>
            ((name[0] <= 'Z' && name[0] >= 'A') || (name[0] <= 'z' && name[0] >= 'a') || (name[0] == '_')) &&
            (name.All(c => !".,/';[](){}*&^%$#@!~`+=-\\|/<>!№:? ".Contains(c)));


        private static Instruction GetCommandInstructions(string inStr)
        {
            List<Command> resultCommands = new List<Command>();
            string[] commandsArr = inStr.Split(new[] {" | "}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < commandsArr.Length; ++i)
            {
                try
                {
                    resultCommands.Add(CreateCommand(commandsArr[i],i));
                }
                catch (CommandException e)
                {
                    throw e;
                }
            }

            for (int i = 1; i < resultCommands.Count; ++i)
            {
                resultCommands[i].Arguments.Add(Path.GetTempPath() + "\\tmpResult" + (i - 1) + ".txt");
            }

            return new Instruction(InstructionType.Command,resultCommands);
        }
        
        private static Command CreateCommand(string command, int numberOfCommand)
        {
            string[] wordsArr = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string name = wordsArr[0].ToLower();
            List<string> args = new List<string>();
            for (int i = 1; i < wordsArr.Length; ++i)
            {
                args.Add(wordsArr[i]);
            }

            switch (name)
            {
                case "echo":
                {
                    Command result = new Echo(args, Path.GetTempPath()+"\\tmpResult"+numberOfCommand+".txt",numberOfCommand == 0);
                    if (result.IsCorrectArgs())
                    {
                        return result;
                    }
                    break;
                }
                case "cat":

                {
                    Command result = new Cat(args, Path.GetTempPath() + "\\tmpResult" + numberOfCommand + ".txt", numberOfCommand == 0);
                    if (result.IsCorrectArgs())
                    {
                        return result;
                    }
                    break;
                    }
                case "pwd":
                {
                    Command result = new Pwd(args, Path.GetTempPath() + "\\tmpResult" + numberOfCommand + ".txt", numberOfCommand == 0);
                    if (result.IsCorrectArgs())
                    {
                        return result;
                    }
                    break;
                    }
                case "wc":
                {
                    Command result = new Wc(args, Path.GetTempPath() + "\\tmpResult" + numberOfCommand + ".txt",
                        numberOfCommand == 0);
                    if (result.IsCorrectArgs())
                    {
                        return result;
                    }

                    break;
                }
                case "exit":
                {
                    Command result = new Exit(args);
                    if (result.IsCorrectArgs())
                    {
                        return result;
                    }

                    break;
                }
            }
            throw new CommandException("Incorrect command name");
        }


    }
}
