using System;
using System.IO;
using System.Net;
using System.Text;
using Horst.Nodes;
using Horst.Parser;

namespace Horst
{
    class Program
    {
        static void Main(string[] args)
        {
            string code =
                
                @"print(httpPost(""https://postman-echo.com/post"", ""This is a test that posts this string to a Web server.""))";
            
            Parser.Parser parser = new Parser.Parser(new TokenStream(new InputStream(code)));
            SequenceNode ast = parser.Parse();
            Environment globalEnv = new StandardLibrary().Environment; 
            Interpreter.Evaluate(ast, globalEnv);
        }
    }
}