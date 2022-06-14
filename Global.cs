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
}
