using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    class Lexer
    {
        public static Lexer Create(string input)
        {
            var lexer = new Lexer(input);
            lexer.ReadChar();
            return lexer;
        }

        static Lexer()
        {

        }
        private Lexer(string input)
        {
            Input = input;
        }
        public void ReadChar()
        {
            if (ReadPosition >= Input.Length)
            {
                CharValue = (char)0;
            }
            else
            {
                CharValue = Input[ReadPosition];
            }
            Position = ReadPosition;
            ReadPosition += 1;
        }
        public Token NextToken()
        {
            Token token = null;
            SkipWhitespace();
            switch (CharValue)
            {
                case '=':
                    token = new Token(TokenEnum.ASSIGN, CharValue);
                    break;
                case ';':
                    token = new Token(TokenEnum.SEMICOLON, CharValue);
                    break;
                case '(':
                    token = new Token(TokenEnum.LPAREN, CharValue);
                    break;
                case ')':
                    token = new Token(TokenEnum.RPAREN, CharValue);
                    break;
                case ',':
                    token = new Token(TokenEnum.COMMA, CharValue);
                    break;
                case '+':
                    token = new Token(TokenEnum.PLUS, CharValue);
                    break;
                case '{':
                    token = new Token(TokenEnum.LBRACE, CharValue);
                    break;
                case '}':
                    token = new Token(TokenEnum.RBRACE, CharValue);
                    break;
                case '0':
                    token = new Token(TokenEnum.EOF, string.Empty);
                    break;
                default:
                    token = new Token();

                    if (IsLetter(CharValue))
                    {
                        token.Literal = ReadIdentifier();
                        token.TokenEnum = LookupIdent(token.Literal);
                        token.TokenType = token.TokenEnum.ToString();
                        return token;
                    }
                    else if (IsDigit(CharValue))
                    {
                        token.Literal = ReadNumber();
                        token.TokenEnum = TokenEnum.INT;
                        token.TokenType = token.TokenEnum.ToString();
                        return token;
                    }
                    else
                    {
                        token = new Token(TokenEnum.ILLEGAL, CharValue);
                    }
                    break;
            }
            ReadChar();
            return token;
        }
        public string ReadIdentifier()
        {
            var position = Position;
            int count = 0;
            while (IsLetter(CharValue))
            {
                ReadChar();
                count++;
            }
            return Input.Substring(position, count);
        }
        public bool IsLetter(char ch)
        {
            var x = 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
            return x;
        }
        public void SkipWhitespace()
        {
            while (Char.IsWhiteSpace(CharValue) || CharValue == '\t' || CharValue == '\n' || CharValue == '\r')
            {
                ReadChar();
            }
        }
        public TokenEnum LookupIdent(string iddent)
        {
            if (Global.KeyWords.TryGetValue(iddent, out TokenEnum token))
            {
                return token;
            }
            return TokenEnum.IDENT;
        }
        public string ReadNumber()
        {
            var position = Position;
            int count = 0;
            while (IsDigit(CharValue))
            {
                ReadChar();
                count++;
            }
            return Input.Substring(position, count);
        }
        public bool IsDigit(char ch)
        {
            //  char.IsDigit(ch);
            return '0' <= ch && ch <= '9';
        }
        public string Input { get; set; }
        public int Position { get; set; }
        public int ReadPosition { get; set; }
        public char CharValue { get; set; }

    }
    enum TokenEnum
    {
        ILLEGAL,
        EOF,
        IDENT,
        INT,
        ASSIGN,
        PLUS,
        COMMA,
        SEMICOLON,
        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,
        FUNCTION,
        LET,
    }
    static class Global
    {
        public static readonly Token[] Tokens = new Token[]
        {
            new Token (TokenEnum.ILLEGAL,"ILLEGAL"),
            new Token (TokenEnum.IDENT,"IDENT"),
            new Token (TokenEnum.INT,"INT"),
            new Token (TokenEnum.ASSIGN,"="),
            new Token (TokenEnum.PLUS,"+"),
            new Token (TokenEnum.COMMA,","),
            new Token (TokenEnum.SEMICOLON,";"),
            new Token (TokenEnum.LPAREN,"("),
            new Token (TokenEnum.RPAREN,")"),
            new Token (TokenEnum.LBRACE,"{"),
            new Token (TokenEnum.RBRACE,"}"),
            new Token (TokenEnum.FUNCTION,"FUNCTION"),
            new Token (TokenEnum.LET,"LET"),
        };
        public static readonly Dictionary<string, TokenEnum> KeyWords = new Dictionary<string, TokenEnum>()
        {
            { "fn",   TokenEnum.FUNCTION},
            { "let",  TokenEnum.LET}
        };
    }

    class Token
    {
        public Token(TokenEnum token, char literal)
        {
            TokenEnum = token;
            Literal = literal.ToString();
            this.TokenType = token.ToString();
        }
        public Token(TokenEnum token, string literal)
        {
            TokenEnum = token;
            Literal = literal;
            this.TokenType = token.ToString();
        }
        public Token(string token, string literal)
        {
            this.TokenType = token;
            this.TokenEnum = (TokenEnum)Enum.Parse(typeof(TokenEnum), token);
            this.Literal = literal;
        }
        public Token()
        {

        }
        public string TokenType { get; set; } = String.Empty;

        public TokenEnum TokenEnum { get; set; } = TokenEnum.ILLEGAL;

        public string Literal { get; set; } = String.Empty;

        public void Console()
        {
            System.Console.WriteLine($"Token is   {TokenEnum},  Value is  {Literal}");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var x = "let five = 5; " +
                "let ten = 10;" +
                "let add = fn(x, y) " +
                "{x + y;};" +
                "let result = add(five, ten);";
            var m = Lexer.Create(x);
            List<Token> tokens = new List<Token>();
            while (m.ReadPosition <= x.Length)
            {
                var tok = m.NextToken();
                tok.Console();
                tokens.Add(tok);
            }
            Console.ReadKey();
        }
    }
}
