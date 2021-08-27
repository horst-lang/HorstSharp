using System;
using System.Collections.Generic;
using System.IO;
using Horst;

namespace HorstRunner
{
    class Program
    {
        private static Command RUN;
        private static List<Command> commandList;
        
        static void Main(string[] args)
        {
            RUN = new Command("run", "Executes the specified file or main.horst in specified direcory", "run <path>",
                () =>
                {
                    try
                    {
                        if (!File.Exists("./main.horst"))
                        {
                            Console.WriteLine("No file main.horst");
                            return;
                        }
                        Runner.Run(File.ReadAllText("./main.horst"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.ReadKey();
                    }
                });

            commandList = new List<Command>()
            {
                RUN
            };
            HandleInput(new []{"run"});
            Console.ReadKey();
        }

        private static void HandleInput(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            for (int i = 0; i < commandList.Count; i++)
            {
                CommandBase commandBase = commandList[i] as CommandBase;

                if (args[0] == commandBase?.CommandId)
                {
                    if (commandList[i] != null)
                    {
                        commandList[i]?.Invoke();
                    }
                }
            }
        }
    }
}