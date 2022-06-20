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
                case '.':
                    token = new Token(TokenEnum.OP, CharValue);
                    break;
                case '"':
                    token = new Token(TokenEnum.STRING, ReadString());
                    break;
                case '[':
                    token = new Token(TokenEnum.LBRACKET, CharValue);
                    break;
                case ']':
                    token = new Token(TokenEnum.RBRACKET, CharValue);
                    break;
                case ':':
                    token = new Token(TokenEnum.COLON, CharValue);
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
                        token.TokenEnum = TokenEnum.Double;
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
        public string ReadString()
        {
            int count = 0;
            var pos = Position + 1;
            while (true)
            {
                ReadChar();
                count++;
                if (Input[this.Position] == '"' || Input[this.Position] == char.MinValue)
                {
                    count--;
                    break;
                }

            }
            return Input.Substring(pos, count);
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
        /// 读取浮点数
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
            if (CharValue == '.')
            {
                ReadChar();
                count++;
            }
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
}
