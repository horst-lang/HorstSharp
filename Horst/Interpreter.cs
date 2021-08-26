using System;
using Horst.Nodes;

namespace Horst
{
    public static class Interpreter
    {
        
        
        public static dynamic Evaluate(Node exp, Environment env)
        {
            switch (exp.Type)
            {
                case NodeType.Number: return ((NumberNode) exp).Value;
                case NodeType.String: return ((StringNode) exp).Value;
                case NodeType.Boolean: return ((BooleanNode) exp).Value;
                case NodeType.Identifier: return env.Get(((IdentifierNode) exp).Value);
                case NodeType.Assignment:
                {
                    AssignmentNode _exp = (AssignmentNode) exp;
                    if (_exp.Left.Type != NodeType.Identifier)
                    {
                        throw new Exception("Cannot assign to " + _exp.Left.Type);
                    }

                    return env.Set(((IdentifierNode) _exp.Left).Value, Evaluate(_exp.Right, env));
                }
                case NodeType.BinaryExpression:
                {
                    BinaryExpressionNode _exp = (BinaryExpressionNode) exp;
                    return ApplyOperator(_exp.Operator, Evaluate(_exp.Left, env), Evaluate(_exp.Right, env));
                }
                case NodeType.Function: return MakeFunction(env, (FunctionNode) exp);
                case NodeType.Condition:
                {
                    ConditionNode _exp = (ConditionNode) exp;
                    dynamic cond = Evaluate(_exp.Condition, env);
                    if (cond != false)
                    {
                        return Evaluate(_exp.Then, env);
                    }

                    return _exp.Else != null ? Evaluate(_exp.Else, env) : false;
                }
                case NodeType.Sequence:
                {
                    dynamic val = false;
                    SequenceNode _exp = (SequenceNode) exp;
                    foreach (var node in _exp.Sequence)
                    {
                        val = Evaluate(node, env);
                    }

                    return val;
                }
                case NodeType.FunctionCall:
                {
                    FunctionCallNode _exp = (FunctionCallNode) exp;
                    var func = Evaluate(_exp.Func, env);
                    return func.Invoke(Array.ConvertAll(_exp.Args, arg => Evaluate(arg, env)));
                }
                default: throw new Exception("I don't know how to evaluate " + exp.Type);
            }
        }

        private static dynamic ApplyOperator(string op, dynamic a, dynamic b)
        {
            switch (op)
            {
                case "+"  : return Num(a) + Num(b);
                case "-"  : return Num(a) - Num(b);
                case "*"  : return Num(a) * Num(b);
                case "/"  : return Num(a) / Div(b);
                case "%"  : return Num(a) % Div(b);
                case "&&" : return a != false && b;
                case "||" : return a != false ? a : b;
                case "<"  : return Num(a) < Num(b);
                case ">"  : return Num(a) > Num(b);
                case "<=" : return Num(a) <= Num(b);
                case ">=" : return Num(a) >= Num(b);
                case "==" : return a == b;
                case "!=" : return a != b;
            }

            throw new Exception("Can't apply operator " + op);
        }

        private static double Num(dynamic x)
        {
            if (x.GetType() != typeof(double))
            {
                throw new Exception("Expected number but got " + x);
            }

            return x;
        }

        private static double Div(double x)
        {
            if (Num(x) == 0)
            {
                throw new DivideByZeroException();
            }

            return x;
        }

        private static Func<dynamic[], dynamic> MakeFunction(Environment env, FunctionNode exp)
        {
            Func<dynamic[], dynamic> func = arguments =>
            {
                string[] names = exp.Vars;
                Environment scope = env.Extend();
                for (var i = 0; i < names.Length; ++i)
                    scope.Define(names[i], i < arguments.Length ? arguments[i] : false);
                return Evaluate(exp.Body, scope);
            };
            return func;
        }
    }
}