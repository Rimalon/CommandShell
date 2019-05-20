using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Commands;

namespace CommandShell
{
    class Interpreter
    {
        private bool IsExit { get; set; }
        private Dictionary<string, object> Variables { get; }

        public Interpreter()
        {
            IsExit = false;
            Variables = new Dictionary<string, object>();
        }

        public void Start()
        {
            Console.WriteLine("You can use:\n" +
                              "echo - display the argument (s)\n" +
                              "exit - exit the interpreter\n" +
                              "pwd - display the current working directory\n" +
                              "cat [FILENAME] - show the contents of a file on the screen\n" +
                              "wc [FILENAME] - show on the screen the number of lines, words and bytes in the file\n\n" +
                              "If your command is not recognized, an attempt will be made with cmd\n\n" +
                              "also you can use:\n" +
                              "operator $ - assignment and use of local session variables\n" +
                              "operator | - pipelining commands. The result of executing one command becomes an input for the other\n");

            while (!IsExit)
            {
                Console.Write("{0}>", Dns.GetHostName());
                string inputStr = Console.ReadLine();

                try
                {
                    ExecuteInstructions(Parser.Parse(inputStr));
                }

                catch (VariableDefException e)
                {
                    Console.WriteLine(e.Message);
                }

                catch (InvalidOperationException)
                {
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }
        }

        private void ExecuteInstructions(Instruction instructions)
        {
            switch (instructions.Type)
            {
                case InstructionType.VariableDef:
                {
                    Variables.Add(instructions.VariableName, instructions.VariableValue);
                    break;
                }
                case InstructionType.CmdCommand:
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            RedirectStandardInput = true,
                            UseShellExecute = false,
                            Arguments = "/C " + instructions.Commands[0].Arguments[0]
                        }
                    };
                    process.Start();
                    process.WaitForExit();
                        break;
                }

                case InstructionType.Command:
                {
                    bool isCorrect = true;
                    foreach (var command in instructions.Commands)
                    {
                        for (int i = 0; i < command.Arguments.Count; ++i)
                        {
                            string tmpVarName = command.Arguments[i].Remove(0, 1);
                            if (Variables.ContainsKey(tmpVarName))
                            {
                                command.Arguments[i] = (string) Variables[tmpVarName];
                            }
                        }
                    }

                    try
                    {
                        foreach (var command in instructions.Commands)
                        {
                            command.Execute();
                        }
                    }
                    catch (CommandException e)
                    {
                        if (e.Message == "Exit")
                        {
                            IsExit = true;
                        }
                        else
                        {
                            isCorrect = false;
                            Console.WriteLine(e.Message);
                        }
                    }

                    if (!IsExit && isCorrect)
                    {
                        using (var reader = File.OpenText(instructions.Commands.Last().TargetPath))
                        {
                            Console.WriteLine(reader.ReadToEnd());
                        }
                    }

                    for (int i = 0; i < instructions.Commands.Count; ++i)
                    {
                        File.Delete(Path.GetTempPath() + "\\tmpResult" + i + ".txt");
                    }

                    break;
                }
            }
        }
    }
}