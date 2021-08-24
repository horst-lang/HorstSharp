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
            char character = input[pos++];
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