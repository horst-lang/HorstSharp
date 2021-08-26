using System;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Horst.Parser
{
    public class InputStream
    {
        public int pos = 0, line = 1, column = 1;
        private string input;

        public InputStream(string input)
        {
            this.input = input;
        }

        public char Next()
        {
            var i = pos++;
            if (i >= 0 && input.Length > i)
            {
                char character = input[i];
                if (character == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }

                return character;
            }

            Error("File can't be empty");
            throw new Exception();
        }

        public char Peek()
        {
            return input[pos];
        }

        public bool EOF()
        {
            return input.Length <= pos;
        }

        public void Error(string msg)
        {
            throw new Exception($"({line}, {column}): {msg}");
        }
    }
}