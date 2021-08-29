using System;
using System.Collections.Generic;
using Horst.Nodes;
using Horst.Tokens;

namespace Horst.Parser
{
    public class Parser
    {
        private readonly TokenStream _input;
        private static readonly BooleanNode False = new BooleanNode(false);
        private readonly Dictionary<string, int> _precedence;
        
        
        public Parser(TokenStream input)
        {
            this._input = input;
            _precedence = new Dictionary<string, int>();
            SetPrecedence();
        }

        private void SetPrecedence()
        {
            _precedence["="] = 1;
            _precedence["||"] = 2;
            _precedence["&&"] = 3; 
            _precedence["<"] = 7; _precedence[">"] = 7; _precedence["<="] = 7; _precedence[">="] = 7; _precedence["=="] = 7; _precedence["!="] = 7;
            _precedence["+"] = 10; _precedence["-"] = 10;
            _precedence["*"] = 20; _precedence["/"] = 20; _precedence["%"] = 20;
        }
        
        // Utilities

        private bool IsPunc(char ch)
        {
            Token tok = _input.Peek();
            return tok != null && tok.Type == TokenType.Punctuation && (tok?.Value == ch) && tok != null;
        }
        private bool IsKeyword(string kw)
        {
            Token tok = _input.Peek();
            return tok != null && tok.Type == TokenType.Keyword && (kw == null || tok?.Value == kw) && tok != null;
        }
        private Token IsOperator()
        {
            Token tok = _input.Peek();
            if (tok == null)
            {
                return null;
            }

            return tok.Type == TokenType.Operator ? tok : null;
        }

        private void SkipPunc(char ch)
        {
            if (IsPunc(ch))
            {
                _input.Next();
            }
            else
            {
                _input.Error("Expecting punctuation: \"" + ch + "\"");
            }
        }
        private void SkipKeyword(string kw)
        {
            if (IsKeyword(kw))
            {
                _input.Next();
            }
            else
            {
                _input.Error("Expecting keyword: \"" + kw + "\"");
            }
        }

        private void Unexpected()
        {
            _input.Error("Unexpected token: " + _input.Peek().Type);
        }

        private FunctionNode ParseFunction()
        {
            return new FunctionNode(Array.ConvertAll(Delimited('(', ')', ',', ParseVarname), o => (string)o), ParseExpression());
        }

        private dynamic[] Delimited(char start, char stop, char seperator, Func<dynamic> parser)
        {
            List<dynamic> a = new List<dynamic>();
            bool first = true;
            
            SkipPunc(start);

            while (!_input.Eof())
            {
                if (IsPunc(stop))
                    break;

                if (first)
                    first = false;
                else
                    SkipPunc(seperator);

                if (IsPunc(stop))
                    break;
                a.Add(parser());
            }

            SkipPunc(stop);
            var res = a.ToArray();
            return res;
        }

        private string ParseVarname()
        {
            Token name = _input.Next();
            if (name.Type != TokenType.Variable)
            {
                _input.Error("Expecting variable name");
            }

            return name.Value;
        }

        private ConditionNode ParseIf()
        {
            SkipKeyword("if");
            Node cond = ParseExpression();
            if (!IsPunc('{'))
            {
                SkipKeyword("then");
            }

            Node then = ParseExpression();
            ConditionNode ret = new ConditionNode(cond, then);
            if (IsKeyword("else"))
            {
                _input.Next();
                ret.Else = ParseExpression();
            }

            return ret;
        }

        private BooleanNode ParseBool()
        {
            return new BooleanNode(_input.Next().Value == "true");
        }

        private Node ParseAtom()
        {
            return MaybeCall(() =>
            {
                if (IsPunc('('))
                {
                    _input.Next();
                    Node exp = ParseExpression();
                    SkipPunc(')');
                    return exp;
                }
                
                //TODO: Implement unary operator

                if (IsPunc('{'))
                {
                    return ParseSequence();
                }

                if (IsKeyword("if"))
                {
                    return ParseIf();
                }

                if (IsKeyword("true") || IsKeyword("false"))
                {
                    return ParseBool();
                }

                if (IsKeyword("function") || IsKeyword("fn"))
                {
                    _input.Next();
                    return ParseFunction();
                }

                Token token = _input.Next();
                if (token.Type == TokenType.Variable)
                {
                    return new IdentifierNode(token.Value);
                }

                if (token.Type == TokenType.Number)
                {
                    return new NumberNode(token.Value);
                }

                if (token.Type == TokenType.String)
                {
                    return new StringNode(token.Value);
                }

                Unexpected();
                return null;
            });
        }

        private Node ParseExpression()
        {
            var res = MaybeCall(() =>
            {
                return MaybeBinary(ParseAtom(), 0);
            });
            return res;
        }


        private Node ParseSequence()
        {
            Node[] prog = Array.ConvertAll(Delimited('{', '}', ';', ParseExpression), s => (Node)s);
            if (prog.Length == 0)
            {
                return False;
            }

            if (prog.Length == 1)
            {
                return prog[0];
            }

            return new SequenceNode(prog);

        }
        
        //MAYBE Functions

        private Node MaybeCall(Func<Node> _expr)
        {
            Node expr = _expr();
            return IsPunc('(') ? ParseCall(expr as IdentifierNode) : expr;
        }
        
        private Node MaybeBinary(Node left, int myPrec)
        {
            Token tok = IsOperator();
            if (tok != null)
            {
                int hisPrec = _precedence[tok.Value];
                if (hisPrec > myPrec)
                {
                    _input.Next();
                    Node right = MaybeBinary(ParseAtom(), hisPrec);
                    Node binary = tok.Value == "="
                        ? new AssignmentNode("=", left, right)
                        : new BinaryExpressionNode(tok.Value, left, right);
                    return MaybeBinary(binary, myPrec);
                }
            }

            return left;
        }

        private FunctionCallNode ParseCall(IdentifierNode func)
        {
            return new FunctionCallNode(func, Array.ConvertAll(Delimited('(', ')', ',', ParseExpression), o => (Node)o));
        }

        private SequenceNode ParseToplevel()
        {
            List<Node> prog = new List<Node>();

            while (!_input.Eof())
            {
                prog.Add(ParseExpression());
                
                if (!_input.Eof())
                {
                    SkipPunc(';');
                }
            }

            return new SequenceNode(prog.ToArray());
        }

        public SequenceNode Parse()
        {
            return ParseToplevel();
        }
    }
}