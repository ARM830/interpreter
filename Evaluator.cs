﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{

    interface IMonkeyobject
    {
        MonkeyTypeEnum MonkeyObjectEnumType();
        string MonkeyObjectType();
        string Inspect();
    }
    class MonkeyReturn : IMonkeyobject
    {
        public IMonkeyobject Value { get; set; }
        public string Inspect()
        {
            return Value.Inspect();
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.Return_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    class MonkeyDouble : IMonkeyobject
    {
        public double Value { get; set; }
        public string Inspect()
        {
            return Value.ToString();
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.Double_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    class MonkeyBoolean : IMonkeyobject
    {
        public bool Value { get; set; }
        public string Inspect()
        {
            return Value.ToString();
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.Boolean_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    class MonkeyNull : IMonkeyobject
    {
        public string Inspect()
        {
            return "Null";
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.Null_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    class MonkeyError : IMonkeyobject
    {
        public string Message { get; set; }
        public string Inspect()
        {
            return "error: " + Message;
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.Error_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    class MonkeyFunction : IMonkeyobject
    {
        public List<Identifier> Parameter { get; set; }
        public BlockStatement Body { get; set; }
        public MonkeyEnvironment Env { get; set; }
        public string Inspect()
        {
            string str = string.Empty;
            foreach (var item in Parameter)
            {
                str += item.OutLine();
            }
            str += $"fn\r\n({string.Join(", ", str)}){{\n{Body.OutLine()}\n}}";
            return str;
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.Function_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    class MonkeyString : IMonkeyobject
    {
        public string Value { get; set; }
        public string Inspect()
        {
            return Value;
        }

        public MonkeyTypeEnum MonkeyObjectEnumType()
        {
            return MonkeyTypeEnum.String_Obj;
        }

        public string MonkeyObjectType()
        {
            return Global.MonkeyTypePairs[MonkeyObjectEnumType()];
        }
    }
    internal class Evaluator
    {
        public static Evaluator Create()
        {
            Evaluator eval = new Evaluator();

            return eval;
        }
        static Evaluator()
        {

        }
        private Evaluator()
        {

        }
        private readonly Dictionary<bool, MonkeyBoolean> BoolValuePairs = new Dictionary<bool, MonkeyBoolean>()
        {
            {true,new MonkeyBoolean(){ Value=true} },
            {false,new MonkeyBoolean(){ Value=false} },
        };
        public readonly MonkeyNull monkeyNull = new MonkeyNull();
        public INode Node { get; set; }
        public IMonkeyobject Eval(INode node, MonkeyEnvironment environment)
        {
            Node = node;
            switch (Node)
            {
                case ProgramCode program:
                    return EvalProgram(program, environment);
                case ExpressionStatement expression:
                    return Eval(expression.Expression, environment);
                case DoubleLiteral doubleLiteral:
                    return new MonkeyDouble() { Value = doubleLiteral.Value };
                case BooleanExpression boolean:
                    return BoolValuePairs[boolean.Value];
                case PrefixExpression prefixExpression:
                    var right = Eval(prefixExpression.Right, environment);
                    if (IsError(right))
                    {
                        return right;
                    }
                    return EvalPrefixExpression(prefixExpression.Operator, right, environment);
                case InfixExpression infixExpression:
                    var left = Eval(infixExpression.Left, environment);
                    if (IsError(left))
                    {
                        return left;
                    }
                    right = Eval(infixExpression.Right, environment);
                    if (IsError(right))
                    {
                        return right;
                    }
                    return EvalInfixExpression(infixExpression.Operator, left, right, environment);
                case BlockStatement blockStatement:
                    var block = EvalBlockStatement(blockStatement, environment);
                    if (IsError(block))
                        return block;
                    return block;
                case IFExpression ifExpression:
                    return EvalIfExpression(ifExpression, environment); ;
                case ReturnStatement returnStatement:
                    var val = Eval(returnStatement.Value, environment);
                    if (IsError(val))
                    {
                        return val;
                    }
                    return new MonkeyReturn() { Value = val };
                case LetStatement letStatement:
                    var let = Eval(letStatement.Value, environment);
                    if (IsError(let))
                    {
                        return let;
                    }
                    environment.Set(letStatement.Name.Value, let);
                    return let;
                case Identifier identifier:
                    return EvalIdentifier(identifier, environment);
                case FunctionLiteral functionLiteral:
                    var param = functionLiteral.Parameter;
                    var body = functionLiteral.Body;
                    return new MonkeyFunction() { Body = body, Parameter = param, Env = environment };
                case CallExpression callExpression:
                    var func = Eval(callExpression.Function, environment);
                    if (IsError(func))
                        return func;
                    var arg = EvalExpressions(callExpression.Arguments, environment);
                    if (arg.Count == 1 && IsError(arg[0]))
                        return arg[0];
                    return ApplyFunction(func, arg);
                case StringLiteral stringLiteral:
                    return new MonkeyString() { Value = stringLiteral.Value };
            }
            return null;
        }
        public IMonkeyobject ApplyFunction(IMonkeyobject fun, List<IMonkeyobject> args)
        {
            if (fun is MonkeyFunction monkeyFunction)
            {
                var extendedenv = ExtendFunctionEnv(monkeyFunction, args);
                var eval = Eval(monkeyFunction.Body, extendedenv);
                return UnwarpReturnValue(eval);
            }
            else
            {
                return CreateError($"not a function {fun.MonkeyObjectType()}");
            }

        }
        public MonkeyEnvironment ExtendFunctionEnv(MonkeyFunction monkeyFunction, List<IMonkeyobject> args)
        {
            var env = MonkeyEnvironment.New(monkeyFunction.Env);
            for (int i = 0; i < monkeyFunction.Parameter.Count; i++)
            {
                env.Set(monkeyFunction.Parameter[i].Value, args[i]);
            }
            return env;
        }
        public IMonkeyobject UnwarpReturnValue(IMonkeyobject obj)
        {
            if (obj is MonkeyReturn monkeyReturn)
            {
                return monkeyReturn.Value;
            }
            return obj;
        }
        public List<IMonkeyobject> EvalExpressions(List<IExpression> exps, MonkeyEnvironment environment)
        {
            var result = new List<IMonkeyobject>();
            foreach (var item in exps)
            {
                var eval = Eval(item, environment);
                if (IsError(eval))
                {
                    return new List<IMonkeyobject> { eval };
                }
                result.Add(eval);
            }
            return result;
        }
        public IMonkeyobject EvalIdentifier(Identifier id, MonkeyEnvironment env)
        {
            var tp = env.Get(id.Value);
            if (!tp.Item2)
            {
                return CreateError("id not fine  " + id.Value);
            }
            return tp.Item1;
        }
        public MonkeyError CreateError(params string[] msg)
        {
            return new MonkeyError() { Message = string.Join(" ", msg) };
        }
        public bool IsError(IMonkeyobject monkeyobject)
        {
            if (monkeyobject != null)
            {
                return monkeyobject.MonkeyObjectEnumType() == MonkeyTypeEnum.Error_Obj;
            }
            return false;
        }
        public IMonkeyobject EvalBlockStatement(BlockStatement blockStatement, MonkeyEnvironment environment)
        {
            IMonkeyobject obj = null;
            foreach (var item in blockStatement.Statements)
            {
                obj = Eval(item, environment);
                if (obj != null && obj.MonkeyObjectEnumType() == MonkeyTypeEnum.Return_Obj)
                {
                    return obj;
                }
            }
            return obj;
        }
        public IMonkeyobject EvalProgram(ProgramCode program, MonkeyEnvironment environment)
        {
            IMonkeyobject obj = null;
            foreach (var item in program.Statement)
            {
                obj = Eval(item, environment);
                switch (obj)
                {
                    case MonkeyReturn monkeyReturn:
                        return monkeyReturn.Value;
                    case MonkeyError monkeyError:
                        return monkeyError;
                }

            }
            return obj;
        }
        public bool IsTruthy(IMonkeyobject val)
        {
            switch (val)
            {
                case MonkeyNull monkeyNull:
                    return false;
                case MonkeyBoolean monkeyBoolean:
                    return monkeyBoolean.Value;
                default:
                    return true;
            }
        }
        public IMonkeyobject EvalIfExpression(IFExpression expression, MonkeyEnvironment environment)
        {
            var condiion = Eval(expression.Condition, environment);
            if (IsError(condiion))
            {
                return condiion;
            }
            if (IsTruthy(condiion))
            {
                return Eval(expression.Consequence, environment);
            }
            else if (expression.Alternative != null)
            {
                return Eval(expression.Alternative, environment);
            }
            else
            {
                return monkeyNull;
            }

        }
        public IMonkeyobject EvalBangOperatorExpression(IMonkeyobject right)
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
        public IMonkeyobject EvalMinusPrefiOperatorExpression(IMonkeyobject right)
        {
            if (right.MonkeyObjectType() != MonkeyTypeEnum.Double_Obj.ToString())
            {
                return CreateError("unknown operator", "-", right.MonkeyObjectType());
            }
            return new MonkeyDouble { Value = -(right as MonkeyDouble).Value };
        }
        public IMonkeyobject EvalPrefixExpression(string op, IMonkeyobject right, MonkeyEnvironment environment)
        {
            switch (op)
            {
                case "!":
                    return EvalBangOperatorExpression(right);
                case "-":
                    return EvalMinusPrefiOperatorExpression(right);
                default:
                    return CreateError("unknown operator", op, right.MonkeyObjectType());
            }
        }
        public IMonkeyobject EvalInfixExpression(string op, IMonkeyobject left, IMonkeyobject right, MonkeyEnvironment environment)
        {
            switch (left)
            {
                case IMonkeyobject l when (l.MonkeyObjectEnumType() == MonkeyTypeEnum.Double_Obj) && (right.MonkeyObjectEnumType() == MonkeyTypeEnum.Double_Obj):
                    //switch (op)
                    //{
                    //    case "==":
                    //        return BoolValuePairs[left.Inspect() == right.Inspect()];
                    //    case "!=":
                    //        return BoolValuePairs[left.Inspect() != right.Inspect()];
                    //}
                    return EvalDoubleInfixExpression(op, left, right);
                case IMonkeyobject l when (l.MonkeyObjectEnumType() == MonkeyTypeEnum.String_Obj) && (right.MonkeyObjectEnumType() == MonkeyTypeEnum.String_Obj):
                    return EvalStringInfixExpressoin(op, left, right);
                default:
                    return CreateError("unknown operator", left.MonkeyObjectType(), op, right.MonkeyObjectType());
            }
        }
        public IMonkeyobject EvalStringInfixExpressoin(string op, IMonkeyobject left, IMonkeyobject right)
        {

            switch (op)
            {
                case "!=":
                    return this.BoolValuePairs[(left as MonkeyString).Value != (right as MonkeyString).Value];
                case "==":
                    return this.BoolValuePairs[(left as MonkeyString).Value == (right as MonkeyString).Value];
                case "+":
                    return new MonkeyString { Value = (left as MonkeyString).Value + (right as MonkeyString).Value };
                default:
                    return CreateError("unknown operator", left.MonkeyObjectType(), op, right.MonkeyObjectType());
            }

        }
        public IMonkeyobject EvalDoubleInfixExpression(string op, IMonkeyobject left, IMonkeyobject right)
        {
            var rv = right as MonkeyDouble;
            var lv = left as MonkeyDouble;
            switch (op)
            {
                case "+":
                    return new MonkeyDouble { Value = lv.Value + rv.Value };
                case "-":
                    return new MonkeyDouble { Value = lv.Value - rv.Value };
                case "*":
                    return new MonkeyDouble { Value = lv.Value * rv.Value };
                case "/":
                    return new MonkeyDouble { Value = lv.Value / rv.Value };
                case "<":
                    return BoolValuePairs[lv.Value < rv.Value];
                case ">":
                    return BoolValuePairs[lv.Value > rv.Value];
                case "==":
                    return BoolValuePairs[lv.Value == rv.Value];
                case "!=":
                    return BoolValuePairs[lv.Value != rv.Value];
                default:
                    return CreateError("unknown operator", left.MonkeyObjectType(), op, right.MonkeyObjectType());

            }
        }
        public IMonkeyobject EvalStatement(List<IStatement> statements, MonkeyEnvironment environment)
        {
            IMonkeyobject obj = null;
            foreach (var item in statements)
            {
                obj = Eval(item, environment);
                if (obj is MonkeyReturn monkeyReturn)
                {
                    return monkeyReturn.Value;
                }
            }
            return obj;
        }
    }
}
