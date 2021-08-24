using System;
using Horst.Parser;

namespace Horst
{
    class Program
    {
        static void Main(string[] args)
        {
            InputStream inputStream = new InputStream("if 1+ 3 \"Hey \" i");
            TokenStream tokenStream = new TokenStream(inputStream);
            while (!tokenStream.EOF())
            {
                Console.WriteLine(tokenStream.Next());
            }
        }
    }
}