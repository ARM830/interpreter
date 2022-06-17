using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    internal class Parser
    {


        static Parser()
        {

        }

        private Parser(Lexer lexer)
        {
            Lexer = lexer;
            RegisterPrefix(TokenEnum.IDENT, ParseIdentifier);
            RegisterPrefix(TokenEnum.Double, ParseDoubleLiteral);
            RegisterPrefix(TokenEnum.BANG, ParsePrefixExpression);
            RegisterPrefix(TokenEnum.MINUS, ParsePrefixExpression);
            RegisterPrefix(TokenEnum.TRUE, ParseBooleanExpression);
            RegisterPrefix(TokenEnum.FALSE, ParseBooleanExpression);
            RegisterPrefix(TokenEnum.LPAREN, ParseGroupedExpression);
            RegisterPrefix(TokenEnum.IF, ParseIFExpression);
            RegisterPrefix(TokenEnum.FUNCTION, ParseFunctionLiteral);

            Registerinfix(TokenEnum.PLUS, ParseInfixExpression);
            Registerinfix(TokenEnum.MINUS, ParseInfixExpression);
            Registerinfix(TokenEnum.SLASH, ParseInfixExpression);
            Registerinfix(TokenEnum.ASTERISK, ParseInfixExpression);
            Registerinfix(TokenEnum.EQ, ParseInfixExpression);
            Registerinfix(TokenEnum.Not_EQ, ParseInfixExpression);
            Registerinfix(TokenEnum.LT, ParseInfixExpression);
            Registerinfix(TokenEnum.GT, ParseInfixExpression);
            Registerinfix(TokenEnum.LPAREN, ParseCallExpression);
        }
        public List<IExpression> ParseCallArguments()
        {
            var list = new List<IExpression>();
            if (PeekTokenIs(TokenEnum.RPAREN))
            {
                NextToken();
                return list;
            }
            NextToken();
            list.Add(ParseExpression());
            while (PeekTokenIs(TokenEnum.COMMA))
            {
                NextToken();
                NextToken();
                list.Add(ParseExpression());
            }
            if (!ExpectPeek(TokenEnum.RPAREN))
            {
                return null;
            }
            return list;
        }
        public IExpression ParseCallExpression(IExpression func)
        {
            var exp = new CallExpression() {Token=CurToken,Function=func };
            exp.Arguments = ParseCallArguments();
            return exp;
        }
        public IExpression ParseFunctionLiteral()
        {
            var exp = new FunctionLiteral() { Token = CurToken };
            if (!ExpectPeek(TokenEnum.LPAREN))
            {
                return null;
            }
            exp.Parameter = ParseFunctionParameter();
            if (!ExpectPeek(TokenEnum.LBRACE))
            {
                return null;
            }
            exp.Body = ParseBlockStatement();
            return exp;
        }
        public List<Identifier> ParseFunctionParameter()
        {
            var list = new List<Identifier>();
            if (PeekTokenIs(TokenEnum.RPAREN))
            {
                NextToken();
                return list;
            }
            NextToken();
            var ident=new Identifier() { Token=CurToken,Value=CurToken.Literal};
            list.Add(ident);
            while (PeekTokenIs(TokenEnum.COMMA))
            {
                NextToken();
                NextToken();
                ident = new Identifier() { Token = CurToken, Value = CurToken.Literal };
                list.Add(ident);
            }
            if (!ExpectPeek(TokenEnum.RPAREN))
            {
                return null;
            }
            return list;
        }
        public IExpression ParseIFExpression()
        {
            var exp = new IFExpression() {Token=CurToken };
            if (!ExpectPeek(TokenEnum.LPAREN))
            {
                return null;
            }
            NextToken();
            exp.Condition = ParseExpression();
            if (!ExpectPeek(TokenEnum.RPAREN))
            {
                return null;
            }
            if (!ExpectPeek(TokenEnum.LBRACE))
            {
                return null;
            }
            exp.Consequence = ParseBlockStatement();
            if (PeekTokenIs(TokenEnum.ELSE))
            {
                NextToken();
                if (!ExpectPeek(TokenEnum.LBRACE))
                {
                    return null;
                }
                exp.Alternative=ParseBlockStatement();
            }
            return exp;
        }
        public BlockStatement ParseBlockStatement()
        {
            var block = new BlockStatement() { Token = CurToken };
            NextToken();
            while (!CurTokenIs(TokenEnum.RBRACE) &&!CurTokenIs(TokenEnum.EOF))
            {
                var stmt = ParseStatement();
                if (stmt!=null)
                {
                    block.Statements.Add(stmt);
                }
                NextToken();
            }
            return block;
        }
        public IExpression ParseBooleanExpression()
        {
            return new BooleanExpression() { Token = CurToken, Value =CurTokenIs(TokenEnum.TRUE) };
        }
        public IExpression ParseGroupedExpression()
        {
            NextToken();
            var exp = ParseExpression();
            if (!ExpectPeek(TokenEnum.RPAREN))
            {
                return null;
            }
            return exp;
        }
        public static Parser Create(Lexer lexer)
        {
            var parser = new Parser(lexer);
            parser.NextToken();
            parser.NextToken();
            return parser;
        }
        IExpression ParseIdentifier()
        {
            return new Identifier { Token = CurToken, Value = CurToken.Literal };
        }
        public Lexer Lexer { get; set; }
        public Token CurToken { get; set; }
        public Token PeekToken { get; set; }
        public List<string> Error { get; set; } = new List<string> { };
        public Dictionary<TokenEnum, Func<IExpression>> PrefixExpressionPairs = new Dictionary<TokenEnum, Func<IExpression>>();
        public Dictionary<TokenEnum, Func<IExpression, IExpression>> InfixExpressionPairs = new Dictionary<TokenEnum, Func<IExpression, IExpression>>();

        public void RegisterPrefix(TokenEnum token, Func<IExpression> fn)
        {
            PrefixExpressionPairs[token] = fn;
        }
        public void Registerinfix(TokenEnum token, Func<IExpression, IExpression> fn)
        {
            InfixExpressionPairs[token] = fn;
        }
        public void NextToken()
        {
            CurToken = PeekToken;
            PeekToken = Lexer.NextToken();
        }
        public IStatement ParseStatement()
        {
            IStatement statement = null;
            switch (CurToken.TokenEnum)
            {
                case TokenEnum.LET:
                    statement = ParserLetStatement();
                    break;
                case TokenEnum.RETURN:
                    statement = ParserRuturnStatement();
                    break;
                default:
                    statement = ParseExpressionStatement();
                    break;
            }
            return statement;
        }
        public LetStatement ParserLetStatement()
        {
            var stmt = new LetStatement() { Token = CurToken };
            if (!ExpectPeek(TokenEnum.IDENT))
            {
                CurError(stmt);
                PeekError(TokenEnum.IDENT);
                return null;
            }
            stmt.Name = new Identifier() { Token = CurToken, Value = CurToken.Literal };
            if (!ExpectPeek(TokenEnum.ASSIGN))
            {
                CurError(stmt);
                PeekError(TokenEnum.ASSIGN);
                return null;
            }
            NextToken();
            stmt.Value = ParseExpression();
            if (PeekTokenIs(TokenEnum.SEMICOLON))
            {
                NextToken();
            }
          // while (!CurTokenIs(TokenEnum.SEMICOLON))
          // {
          //     NextToken();
          // }
            return stmt;
        }
        public ReturnStatement ParserRuturnStatement()
        {
            var stmt = new ReturnStatement() { Token = CurToken };
            NextToken();
            stmt.Value = ParseExpression();
            if (PeekTokenIs(TokenEnum.SEMICOLON))
            {
                NextToken();
            }
            //while (!CurTokenIs(TokenEnum.SEMICOLON))
            //{
            //    NextToken();
            //}
            return stmt;
        }
        public bool ExpectPeek(TokenEnum token)
        {
            if (PeekTokenIs(token))
            {
                NextToken();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PeekTokenIs(TokenEnum token)
        {
            return PeekToken.TokenEnum == token;
        }
        public bool CurTokenIs(TokenEnum token)
        {
            return CurToken.TokenEnum == token;
        }
        public ExpressionStatement ParseExpressionStatement()
        {
            ExpressionStatement stmt = new ExpressionStatement() { Token = CurToken };
            stmt.Expression = ParseExpression();
            if (PeekTokenIs(TokenEnum.SEMICOLON))
            {
                NextToken();
            }
            return stmt;
        }
        IExpression ParseExpression(int lowset = 1)
        {
            PrefixExpressionPairs.TryGetValue(CurToken.TokenEnum, out Func<IExpression> prefix);
            if (prefix == null)
            {
                NoPreExpressionError(CurToken.TokenEnum);
                return null;
            }
            var leftexp = prefix();
            while (!PeekTokenIs(TokenEnum.SEMICOLON) && lowset < PeekPrecedence())
            {
                var index = InfixExpressionPairs[PeekToken.TokenEnum];
                if (index == null)
                {
                    return leftexp;
                }
                NextToken();
                leftexp = index(leftexp);
            }
            return leftexp;
        }

        public ProgramCode ParserProgram()
        {
            ProgramCode code = new ProgramCode();
            while (!CurTokenIs(TokenEnum.EOF))
            {
                var stmt = ParseStatement();
                if (stmt != null)
                {
                    code.Statement.Add(stmt);
                }
                NextToken();
            }
            return code;
        }
        public void CurError(IStatement err)
        {
            var error = $"{err.TokenLiteral()} err ";
            Console.WriteLine(error);
        }
        public void PeekError(TokenEnum token)
        {
            var error = $"expected next token to be {token}, got {PeekToken.TokenType} instead ";
            Console.WriteLine(error);
            Error.Add(error);
        }
        public void NoPreExpressionError(TokenEnum token)
        {
            var error = $"no prefix parse function for {token}";
            Console.WriteLine(error);
            Error.Add(error);
        }
        public IExpression ParseDoubleLiteral()
        {
            var lit = new DoubleLiteral() { Token = CurToken };
            double v;
            if (double.TryParse(CurToken.Literal, out v))
            {
                lit.Value = v;
                return lit;
            }
            else
            {
                Error.Add("tryparse double error");
                return null;
            }
        }
        public IExpression ParsePrefixExpression()
        {
            var exp = new PrefixExpression() { Token = CurToken, Operator = CurToken.Literal };
            NextToken();
            exp.Right = ParseExpression((int)Lowset.PREFIX);
            return exp;

        }

        public IExpression ParseInfixExpression(IExpression expression)
        {
            var exp = new InfixExpression() { Token = CurToken, Operator = CurToken.Literal, Left = expression };


            var precedence = CurPrecedence();
            NextToken();
            exp.Right = ParseExpression(precedence);
            return exp;

        }
        public int PeekPrecedence()
        {
            if (Global.Precedences.TryGetValue(PeekToken.TokenEnum, out Lowset lowset))
            {
                return (int)lowset;
            }
            return 1;
        }
        public int CurPrecedence()
        {
            if (Global.Precedences.TryGetValue(CurToken.TokenEnum, out Lowset lowset))
            {
                return (int)lowset;
            }
            return 1;
        }
    }
}
