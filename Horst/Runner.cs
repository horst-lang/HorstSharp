using System;
using System.IO;
using Horst.Nodes;
using Horst.Parser;

namespace Horst
{
    public static class Runner
    {
        public static void Run(string code, string file)
        {
            Parser.Parser parser = new Parser.Parser(new TokenStream(new InputStream(code, file)));
            SequenceNode ast = parser.Parse();
            Environment globalEnv = new StandardLibrary(Path.GetDirectoryName(file)).Environment;
            var res = Interpreter.Evaluate(ast, globalEnv);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n\n*** {Path.GetFileName(file)} finished successfully with: {res}");
            Console.ResetColor();
        }
    }
}