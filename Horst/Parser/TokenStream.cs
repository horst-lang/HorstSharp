using System;
using System.Linq;
using Horst.Tokens;

namespace Horst.Parser
{
    public class TokenStream
    {
        private Token current;
        private InputStream input;
        private static readonly string[] keywords = new[] { "if", "then", "else", "function", "true", "false" };

        // Constructor
        public TokenStream(InputStream input)
        {
            this.input = input;
        }

        private bool IsKeyword(string word)
        {
            return keywords.Contains(word);
        }

        private bool IsIdentifier(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }

        private bool IsOperatorChar(char ch)
        {
            return "+-*/%=&|<>!".Contains(ch);
        }

        private string ReadWhile(Func<char, bool> function)
        {
            string str = "";
            while (!input.EOF() && function(input.Peek()))
            {
                str += input.Next();
            }

            return str;
        }

        private NumberToken ReadNumber()
        {
            bool hasDot = false;
            string number = ReadWhile((ch =>
            {
                if (ch == '.')
                {
                    if (hasDot)
                    {
                        return false;
                    }

                    hasDot = true;
                    return true;
                }

                return char.IsDigit(ch);
            }));

            return new NumberToken(double.Parse(number));
        }

        private Token ReadIdentifier()
        {
            string id = ReadWhile(IsIdentifier);
            return IsKeyword(id) ? new KeywordToken(id) : new VariableToken(id);
        }

        private string ReadEscaped(char end)
        {
            bool escaped = false;
            string str = "";
            input.Next();
            while (!input.EOF())
            {
                char ch = input.Next();
                if (escaped)
                {
                    str += ch;
                    escaped = false;
                } 
                else if (ch == '\\')
                {
                    escaped = true;
                }
                else if (ch == end)
                {
                    break;
                }
                else
                {
                    str += ch;
                }
            }

            return str;
        }

        private StringToken ReadString()
        {
            return new StringToken(ReadEscaped('"'));
        }

        private void SkipComment()
        {
            ReadWhile((c => c != '\n'));
            input.Next();
        }

        private Token ReadNext()
        {
            ReadWhile(char.IsWhiteSpace);
            if (input.EOF()) return null;
            char ch = input.Peek();
            if (ch == '#')
            {
                SkipComment();
                return ReadNext();
            }

            if (ch == '"') return ReadString();
            if (char.IsDigit(ch)) return ReadNumber();
            if (IsIdentifier(ch)) return ReadIdentifier();
            if (char.IsPunctuation(ch)) return new PunctuationToken(input.Next());
            if (IsOperatorChar(ch)) return new OperatorToken(ReadWhile(IsOperatorChar));
            input.Error($"Can't handle Character: {ch}");
            return null;
        }

        public Token Peek()
        {
            return current == null ? (current = ReadNext()) : current;
        }

        public Token Next()
        {
            Token token = current;
            current = null;
            return token == null ? ReadNext() : token;
        }

        public bool EOF()
        {
            return Peek() == null;
        }

        public void Error(string msg)
        {
            input.Error(msg);
        }
    }
}