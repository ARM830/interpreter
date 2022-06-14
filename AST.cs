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
            throw new NotImplementedException();
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
            string str = $"{TokenLiteral()}  {Name.OutLine()} ={Value.OutLine()};";
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
            string str = $"{TokenLiteral()}  ={Value.OutLine()};";
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
            string str = $"{Expression.OutLine()};";
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
            var str = $"{Operator} \r\n{Right.OutLine()}";
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
            var str = $"{Left.OutLine()} \r\n{Operator} \r\n{Right.OutLine()}";
            return str;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
}
