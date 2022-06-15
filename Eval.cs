using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    interface Monkeyobject
    {
        string MonkeyObjectType();
        string Inspect();
    }
    class MonkeyDouble : Monkeyobject
    {
        public double Value { get; set; }
        public string Inspect()
        {
            return Value.ToString();
        }

        public string MonkeyObjectType()
        {
            return MonkeyTypeEnum.Double_Obj.ToString();
        }
    }
    class MonkeyBoolean : Monkeyobject
    {
        public bool Value { get; set; }
        public string Inspect()
        {
            return Value.ToString();
        }

        public string MonkeyObjectType()
        {
            return MonkeyTypeEnum.Boolean_Obj.ToString();
        }
    }
    class MonkeyNull : Monkeyobject
    {
        public string Inspect()
        {
            return "Null";
        }

        public string MonkeyObjectType()
        {
            return MonkeyTypeEnum.Null_Obj.ToString();
        }
    }
    internal class Eval
    {
        public static Eval Create()
        {
            Eval eval = new Eval();

            return eval;
        }
        static Eval()
        {

        }
        private Eval()
        {

        }
        private readonly Dictionary<bool, MonkeyBoolean> BoolValuePairs = new Dictionary<bool, MonkeyBoolean>()
        {
            {true,new MonkeyBoolean(){ Value=true} },
            {false,new MonkeyBoolean(){ Value=false} },
        };
        public readonly MonkeyNull monkeyNull = new MonkeyNull();
        public INode Node { get; set; }
        public Monkeyobject InitEval(INode node)
        {
            Node = node;
            switch (Node)
            {
                case ProgramCode program:
                    return EvalStatement(program.Statement);
                case ExpressionStatement expression:
                    return InitEval(expression.Expression);
                case DoubleLiteral doubleLiteral:
                    return new MonkeyDouble() { Value = doubleLiteral.Value };
                case BooleanExpression boolean:
                    return BoolValuePairs[boolean.Value];
                case PrefixExpression prefixExpression:
                    var right = InitEval(prefixExpression.Right);
                    return EvalPrefixExpression(prefixExpression.Operator, right);
            }
            return null;
        }
        public Monkeyobject EvalBangOperatorExpression(Monkeyobject right)
        {
            switch (right)
            {
                case MonkeyBoolean boolean:
                    return BoolValuePairs[!boolean.Value];
                case null:
                    return BoolValuePairs[true];
                default:
                    return BoolValuePairs[false];
            }
        }
        public Monkeyobject EvalMinusPrefiOperatorExpression(Monkeyobject right)
        {
            if (right.MonkeyObjectType() != MonkeyTypeEnum.Double_Obj.ToString())
            {
                return monkeyNull;
            }
            return new MonkeyDouble { Value = -(right as MonkeyDouble).Value };
        }
        public Monkeyobject EvalPrefixExpression(string op, Monkeyobject right)
        {
            switch (op)
            {
                case "!":
                    return EvalBangOperatorExpression(right);
                case "-":
                    return EvalMinusPrefiOperatorExpression(right);
                default:
                    return monkeyNull;
            }
        }
        public Monkeyobject EvalStatement(List<IStatement> statements)
        {
            Monkeyobject obj = null;
            foreach (var item in statements)
            {
                obj = InitEval(item);
            }
            return obj;
        }
    }
}
