using System;

namespace Horst.Parser
{
    public class InputStream
    {
        public int Pos = 0, Line = 1, Column = 1;
        private readonly string File;
        private readonly string _input;

        public InputStream(string input, string file)
        {
            this._input = input;
            File = file;
        }

        public char Next()
        {
            var i = Pos++;
            if (i >= 0 && _input.Length > i)
            {
                char character = _input[i];
                if (character == '\n')
                {
                    Line++;
                    Column = 1;
                }
                else
                {
                    Column++;
                }

                return character;
            }

            Error("File can't be empty");
            throw new Exception();
        }

        public char Peek()
        {
            return _input[Pos];
        }

        public bool Eof()
        {
            return _input.Length <= Pos;
        }

        public void Error(string msg)
        {
            throw new Exception($"{File} ({Line}, {Column}): {msg}");
        }
    }
}