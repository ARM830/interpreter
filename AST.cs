using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    interface INode
    {
        string TokenLiteral();
        string OutLine();

    }
    interface IStatement : INode
    {
        void StatementNode();
    }
    interface IExpression : INode
    {
        void ExpressionNode();
    }
    class ProgramCode : INode
    {
        public ProgramCode()
        {
            Statement = new List<IStatement>();
        }
        public List<IStatement> Statement { get; set; }

        public string OutLine()
        {
            string str = string.Empty;
            foreach (var item in Statement)
            {
                str += item.OutLine();
            }
            return str;
        }

        public string TokenLiteral()
        {
            if (Statement.Count > 0)
            {
                return Statement[0].TokenLiteral();
            }
            return "";
        }
    }
    class Identifier : IExpression
    {
        public Token Token { get; set; }
        public Identifier Name { get; set; }
        public string Value { get; set; }
        public void ExpressionNode()
        {

        }

        public string OutLine()
        {
            var str = $"{Value}";
            return str;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class LetStatement : IStatement
    {
        public Token Token { get; set; }
        public Identifier Name { get; set; }
        public IExpression Value { get; set; }

        public string OutLine()
        {
            string str = $"{TokenLiteral()}  {Name.OutLine()} ={Value?.OutLine()};";
            return str;
        }

        public void StatementNode()
        {

        }

        public string TokenLiteral()
        {

            return Token.Literal;
        }

    }
    class ReturnStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Value { get; set; }

        public string OutLine()
        {
            string str = $"{TokenLiteral()}  ={Value?.OutLine()};";
            return str;
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class ExpressionStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Expression { get; set; }

        public string OutLine()
        {
            string str = $"{Expression?.OutLine()}";
            return str;
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class DoubleLiteral : IExpression
    {
        public Token Token { get; set; }
        public double Value { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            return Token.Literal;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class PrefixExpression : IExpression
    {
        public Token Token { get; set; }
        public string Operator { get; set; }
        public IExpression Right { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            var str = $"({Operator} {Right?.OutLine()})";
            return str;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }

    class InfixExpression : IExpression
    {
        public Token Token { get; set; }
        public string Operator { get; set; }
        public IExpression Right { get; set; }
        public IExpression Left { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            var str = $"({Left.OutLine()} {Operator}  {Right?.OutLine()})";
            return str;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class BooleanExpression : IExpression
    {
        public Token Token { get; set; }
        public bool Value { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            return Token.Literal;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class BlockStatement : IStatement
    {
        public Token Token { get; set; }

        public List<IStatement> Statements { get; set; } = new List<IStatement>();


        public string OutLine()
        {
            var str = string.Empty;
            foreach (var item in Statements)
            {
                str += item?.OutLine();
            }
            return str;
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class IFExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Condition { get; set; }
        public BlockStatement Consequence { get; set; }
        public BlockStatement Alternative { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            var str = $"if {Condition?.OutLine()} {Consequence?.OutLine()}\r\n";
            str += Consequence != null ? $"else {Consequence.OutLine()}" : "";
            return str;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }

    class FunctionLiteral : IExpression
    {
        public Token Token { get; set; }
        public List<Identifier> Parameter { get; set; } = new List<Identifier>();

        public BlockStatement Body { get; set; }


        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            string str = string.Empty, param = string.Empty;
            foreach (var item in Parameter)
            {
                param += item?.OutLine();
            }
            str += TokenLiteral();
            str += $"({string.Join(",", param)})\r\n{Body.OutLine()}";
            return str;

        }
        public string TokenLiteral()
        {
            return Token.Literal;
        }

    }

    class CallExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Function { get; set; }
        public List<IExpression> Arguments { get; set; }


        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            string str = string.Empty, param = string.Empty;
            foreach (var item in Arguments)
            {
                param += item?.OutLine();
            }
            str += Function.OutLine();
            str += $"({string.Join(",", param)})";
            return str;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class StringLiteral : IExpression
    {
        public Token Token { get; set; }
        public string Value { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            return Token.Literal;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class ArrayLiteral : IExpression
    {
        public Token Token { get; set; }
        public List<IExpression> Element { get; set; } = new List<IExpression>();
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            var ele = new List<string>();
            foreach (var item in Element)
            {
                ele.Add(item.OutLine());
            }
            return $"[{string.Join(",", ele)}]";
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
    class IndexExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Left { get; set; }

        public IExpression Index { get; set; }
        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            return $"({Left?.OutLine()}[{Index?.OutLine()}])";
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
  
    class HashLiteral : IExpression
    {
        public Token Token { get; set; }

        public Dictionary<IExpression, IExpression> Pairs { get; set; }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string OutLine()
        {
            var str = new List<string>();
            foreach (var item in Pairs.Values)
            {
                str.Add(item.OutLine());
            }
            return $"{{{string.Join(",", str)}}}";
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
}
