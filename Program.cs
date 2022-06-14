using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    class Lexer
    {
        /// <summary>
        /// 创建一个新的lexer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Lexer Create(string input)
        {
            var lexer = new Lexer(input);
            lexer.ReadChar();
            return lexer;
        }
        /// <summary>
        /// 不可实例化
        /// </summary>
        static Lexer()
        {

        }
        /// <summary>
        /// 保持权限等级,实例化
        /// </summary>
        /// <param name="input"></param>
        private Lexer(string input)
        {
            Input = input;
        }
        /// <summary>
        /// 读取下一个
        /// </summary>
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
            //交换位置
            Position = ReadPosition;
            //指向下一个
            ReadPosition += 1;
        }
        /// <summary>
        /// 获取下一个token
        /// </summary>
        /// <returns><seealso cref="Token"/></returns>
        public Token NextToken()
        {
            Token token = null;
            //跳过空白字符
            SkipWhitespace();
            //筛选当前字符
            switch (CharValue)
            {
                case '=':
                    if (PeekChar() == '=')
                    {
                        var temp = CharValue;
                        ReadChar();
                        token = new Token(TokenEnum.EQ, temp.ToString() + CharValue);
                    }
                    else
                    {
                        token = new Token(TokenEnum.ASSIGN, CharValue);
                    }

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
                case '-':
                    token = new Token(TokenEnum.MINUS, CharValue);
                    break;
                case '!':
                    if (PeekChar() == '=')
                    {
                        var temp = CharValue;
                        ReadChar();
                        token = new Token(TokenEnum.Not_EQ, temp.ToString() + CharValue);
                    }
                    else
                    {
                        token = new Token(TokenEnum.BANG, CharValue);
                    }
                    break;
                case '*':
                    token = new Token(TokenEnum.ASTERISK, CharValue);
                    break;
                case '/':
                    token = new Token(TokenEnum.SLASH, CharValue);
                    break;
                case '<':
                    token = new Token(TokenEnum.LT, CharValue);
                    break;
                case '>':
                    token = new Token(TokenEnum.GT, CharValue);
                    break;
                case (char)0:
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
        /// <summary>
        /// 读取id
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 是否为字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool IsLetter(char ch)
        {
            var x = 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
            return x;
        }
        /// <summary>
        /// 跳过空白
        /// </summary>
        public void SkipWhitespace()
        {
            while (Char.IsWhiteSpace(CharValue) || CharValue == '\t' || CharValue == '\n' || CharValue == '\r')
            {
                ReadChar();
            }
        }
        /// <summary>
        /// 判断否是为id
        /// </summary>
        /// <param name="iddent"></param>
        /// <returns></returns>
        public TokenEnum LookupIdent(string iddent)
        {
            if (Global.KeyWords.TryGetValue(iddent, out TokenEnum token))
            {
                return token;
            }
            return TokenEnum.IDENT;
        }
        /// <summary>
        /// 读取整数
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 是否为字符
        /// </summary>
        /// <param name="ch">参数</param>
        /// <returns></returns>
        public bool IsDigit(char ch)
        {
            //  char.IsDigit(ch);
            return '0' <= ch && ch <= '9';
        }
        /// <summary>
        /// 向前看一个字符
        /// </summary>
        /// <returns></returns>
        public char PeekChar()
        {
            if (ReadPosition >= Input.Length)
                return (char)0;
            else
                return Input[ReadPosition];
        }
        /// <summary>
        /// 输入的字符
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// 当前字符位置
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 下一个字符位置
        /// </summary>
        public int ReadPosition { get; set; }
        /// <summary>
        /// 当前char
        /// </summary>
        public char CharValue { get; set; }

    }
    enum TokenEnum
    {
        /// <summary>
        /// 非法
        /// </summary>
        ILLEGAL,
        /// <summary>
        /// 结尾
        /// </summary>
        EOF,
        /// <summary>
        /// id
        /// </summary>
        IDENT,
        /// <summary>
        /// 整数
        /// </summary>
        INT,
        /// <summary>
        /// 等于 =
        /// </summary>
        ASSIGN,
        /// <summary>
        /// +
        /// </summary>
        PLUS,
        /// <summary>
        /// ,
        /// </summary>
        COMMA,
        /// <summary>
        /// ;
        /// </summary>
        SEMICOLON,
        /// <summary>
        /// (
        /// </summary>
        LPAREN,
        /// <summary>
        /// )
        /// </summary>
        RPAREN,
        /// <summary>
        /// {
        /// </summary>
        LBRACE,
        /// <summary>
        /// }
        /// </summary>
        RBRACE,
        /// <summary>
        /// fn
        /// </summary>
        FUNCTION,
        /// <summary>
        /// let
        /// </summary>
        LET,
        /// <summary>
        /// -
        /// </summary>
        MINUS,
        /// <summary>
        /// !
        /// </summary>
        BANG,
        /// <summary>
        /// *
        /// </summary>
        ASTERISK,
        /// <summary>
        /// /
        /// </summary>
        SLASH,
        /// <summary>
        ///  <![CDATA[<]]>
        /// </summary>
        LT,
        /// <summary>
        /// <![CDATA[>]]>
        /// </summary>
        GT,
        /// <summary>
        /// true
        /// </summary>
        TRUE,
        /// <summary>
        /// false
        /// </summary>
        FALSE,
        /// <summary>
        /// if
        /// </summary>
        IF,
        /// <summary>
        /// else
        /// </summary>
        ELSE,
        /// <summary>
        /// return
        /// </summary>
        RETURN,
        /// <summary>
        /// eq
        /// </summary>
        EQ,
        /// <summary>
        /// not eq
        /// </summary>
        Not_EQ
    }
    static class Global
    {
        public static readonly Dictionary<char, Token> TokenPairs = new Dictionary<char, Token>()
        {
            {'=',new Token (TokenEnum.ASSIGN,"=") },
            {'+',new Token (TokenEnum.PLUS,"+") },
            {',',new Token (TokenEnum.COMMA,",") },
            {';',new Token (TokenEnum.SEMICOLON,";") },
            {'(',new Token (TokenEnum.LPAREN,"(") },
            {')',new Token (TokenEnum.RPAREN,")") },
            {'{',new Token (TokenEnum.LBRACE,"{") },
            {'}',new Token (TokenEnum.RBRACE,"}") },
            {'-',new Token (TokenEnum.MINUS,"-") },
            {'!',new Token (TokenEnum.BANG,"!") },
            {'*',new Token (TokenEnum.ASTERISK,"*") },
            {'/',new Token (TokenEnum.SLASH,"/") },
            {'<',new Token (TokenEnum.LT,"<") },
            {'>',new Token (TokenEnum.GT,">") },
            // char.MinValue
            {(char)0,new Token(TokenEnum.EOF,string.Empty) }

        };
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
            new Token (TokenEnum.MINUS,"-"),
            new Token (TokenEnum.BANG,"!"),
            new Token (TokenEnum.ASTERISK,"*"),
            new Token (TokenEnum.SLASH,"/"),
            new Token (TokenEnum.LT,"<"),
            new Token (TokenEnum.GT,">"),
            new Token (TokenEnum.EQ,"=="),
            new Token (TokenEnum.Not_EQ,"!="),
        };
        public static readonly Dictionary<string, TokenEnum> KeyWords = new Dictionary<string, TokenEnum>()
        {
            { "fn",   TokenEnum.FUNCTION},
            { "let",  TokenEnum.LET},
            { "true",  TokenEnum.TRUE},
            { "false",  TokenEnum.FALSE},
            { "if",  TokenEnum.IF},
            { "else",  TokenEnum.ELSE},
            {"return",TokenEnum. RETURN},
        };
    }
    /// <summary>
    /// token类
    /// </summary>
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
        /// <summary>
        /// 枚举字符
        /// </summary>
        public string TokenType { get; set; } = String.Empty;
        /// <summary>
        /// 枚举类型
        /// </summary>
        public TokenEnum TokenEnum { get; set; } = TokenEnum.ILLEGAL;
        /// <summary>
        /// 含义
        /// </summary>
        public string Literal { get; set; } = String.Empty;
        /// <summary>
        /// 输出
        /// </summary>
        public void Console()
        {
            System.Console.WriteLine($"Token is   {TokenEnum},  Value is  {Literal}");
        }
    }
    /// <summary>
    /// 第一章
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            var x = "let five = 5; " +
                "let ten = 10;" +
                "let add = fn(x, y) " +
                "{x + y;};" +
                "let result = add(five, ten);" +
                "!-/*5;5 < 10 > 5;" +
                "if (5 < 10){return true;} " +
                "else{return false;}" +
                "10 == 10;" +
                "10 != 9; ";
            var m = Lexer.Create(x);
            List<Token> tokens = new List<Token>();
            var tok = m.NextToken();
            while (tok.TokenEnum != TokenEnum.EOF)
            {
                tok = m.NextToken();
                tok.Console();
                tokens.Add(tok);
            }

            Console.ReadKey();
        }
    }
}
