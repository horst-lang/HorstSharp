using System;
using Horst.Nodes;
using Horst.Parser;

namespace Horst
{
    public static class Runner
    {
        public static void Run(string code)
        {
            Parser.Parser parser = new Parser.Parser(new TokenStream(new InputStream(code)));
            SequenceNode ast = parser.Parse();
            Environment globalEnv = new StandardLibrary().Environment; 
            Interpreter.Evaluate(ast, globalEnv);
            Console.ReadKey();
        }
    }
}