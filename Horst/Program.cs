using System;
using System.Collections.Generic;
using System.IO;
using Horst.Commands;

namespace Horst
{
    static class Program
    {
        
        const string Version = "v0.1.0";
        const string HorstLogo = @"
                                    ./#&&%#/.                                   
                          #&&&&&&&&&&&&&&&&&&&&&&&&&&&                          
                     &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&                          
                 &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&         &                
              &&&&&&&&&&& &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&             
           &&&&&&&&&,     &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&#          
         &&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&%        
       &&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&#      
     .&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&     
    %&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&(   
   &&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&#  
  &&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&/ 
 .&&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&& 
 &&&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&&&
 &&&&&&&&&&&&&&&&,        %%%%%%%%%%%%%%%%%%%%%%%%%%%%         &&&&&&&&&&&&&&&&&
#&&&&&&&&&&&&&&&&,                                             &&&&&&&&&&&&&&&&&
&&&&&&&&&&&&&&&&&,                                             &&&&&&&&&&&&&&&&&
(&&&&&&&&&&&&&&&&,                                             &&&&&&&&&&&&&&&&&
 &&&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&&&
 &&&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&&%
  &&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&& 
  /&&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&&. 
   #&&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&&,  
    *&&&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&&    
      &&&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&&&     
       *&&&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&&&       
         (&&&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&         &&&&&&&&,        
           .&&&&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&&      %&&&&&&&&           
              %&&,        &&&&&&&&&&&&&&&&&&&&&&&&&&&& ,&&&&&&&&&&/             
                          &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&.                
                          &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&                     
                           /&&&&&&&&&&&&&&&&&&&&&&&&&*                          ";
        
        
        
        private static Command<string> _run;
        private static Command _help;
        private static List<object> _commandList;
        
        static void Main(string[] args)
        {
            
            
            _run = new Command<string>("run", "Executes the specified file or main.horst in specified directory", "run <path>",
                (path) =>
                {
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            if (File.Exists(Path.Combine(path, "./main.horst")))
                            {
                                Runner.Run(File.ReadAllText(Path.Combine(path, "./main.horst")), Path.GetFullPath(Path.Combine(path, "./main.horst")));
                            }
                            else
                            {
                                Console.WriteLine($"No main.horst file at directory {Path.GetFullPath(path)}");
                            }
                        }
                        else if (File.Exists(path))
                        {
                            if (Path.GetExtension(path) == ".horst")
                            {
                                Runner.Run(File.ReadAllText(path), Path.GetFullPath(path));
                            }
                            else
                            {
                                Console.WriteLine($"{Path.GetFullPath(path)} is not a .horst file");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Path {Path.GetFullPath(path)} does not exist");
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                    }
                });

            _help = new Command("help", "Shows a list of commands", "help", () =>
            {
                for (int i = 0; i < _commandList.Count; i++)
                {
                    CommandBase command = _commandList[i] as CommandBase;

                    Console.WriteLine($"{command?.CommandFormat} - {command?.CommandDescription}");
                }
            });

            _commandList = new List<object>()
            {
                _run,
                _help
            };
            HandleInput(args);
        }

        private static void HandleInput(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(HorstLogo);
                Console.WriteLine($"Horst {Version}");
                return;
            }
            
            for (int i = 0; i < _commandList.Count; i++)
            {
                CommandBase commandBase = _commandList[i] as CommandBase;

                if (args[0] == commandBase?.CommandId)
                {
                    if (_commandList[i] is Command)
                    {
                        (_commandList[i] as Command)?.Invoke();
                    }
                    else if(_commandList[i] is Command<string>)
                    {
                        (_commandList[i] as Command<string>)?.Invoke(args[1]);
                    }
                    else
                    {
                        Console.WriteLine("Command not found. Try 'help' for a list of commands");
                    }
                }
            }
        }
    }
}