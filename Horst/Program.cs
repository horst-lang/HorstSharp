using System;
using Horst.Nodes;
using Horst.Parser;

namespace Horst
{
    class Program
    {
        static void Main(string[] args)
        {
            InputStream inputStream = new InputStream("x * 3");
            try
            {
                TokenStream tokenStream = new TokenStream(inputStream);
                Parser.Parser parser = new Parser.Parser(tokenStream);
                SequenceNode res = parser.Parse();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}