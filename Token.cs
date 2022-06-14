using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
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
}
