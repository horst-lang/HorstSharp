﻿using System;
using System.Linq;
using Horst.Tokens;

namespace Horst.Parser
{
    public class TokenStream
    {
        private Token _current;
        private readonly InputStream _input;
        private static readonly string[] Keywords = new[] { "if", "then", "else", "function", "fn", "true", "false" };

        // Constructor
        public TokenStream(InputStream input)
        {
            this._input = input;
        }

        private bool IsKeyword(string word)
        {
            return Keywords.Contains(word);
        }

        private bool IsIdentifier(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }

        private bool IsOperatorChar(char ch)
        {
            return "+-*/%=&|<>!".Contains(ch);
        }     
        private bool IsPunctuation(char ch)
        {
            return ",;(){}[]".Contains(ch);
        }

        private string ReadWhile(Func<char, bool> function)
        {
            string str = "";
            while (!_input.Eof() && function(_input.Peek()))
            {
                str += _input.Next();
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
            _input.Next();
            while (!_input.Eof())
            {
                char ch = _input.Next();
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
            _input.Next();
        }

        private Token ReadNext()
        {
            ReadWhile(char.IsWhiteSpace);
            if (_input.Eof()) return null;
            char ch = _input.Peek();
            if (ch == '#')
            {
                SkipComment();
                return ReadNext();
            }

            if (ch == '"') return ReadString();
            if (char.IsDigit(ch)) return ReadNumber();
            if (IsIdentifier(ch)) return ReadIdentifier();
            if (IsPunctuation(ch)) return new PunctuationToken(_input.Next());
            if (IsOperatorChar(ch)) return new OperatorToken(ReadWhile(IsOperatorChar));
            _input.Error($"Can't handle Character: {ch}");
            return null;
        }

        public Token Peek()
        {
            return _current ??= ReadNext();
        }

        public Token Next()
        {
            Token token = _current;
            _current = null;
            return token ?? ReadNext();
        }

        public bool Eof()
        {
            return Peek() == null;
        }

        public void Error(string msg)
        {
            _input.Error(msg);
        }
    }
}