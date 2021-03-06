using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    enum MonkeyTypeEnum
    {
        Boolean_Obj,
        Double_Obj,
        Null_Obj,
        Return_Obj,
        Error_Obj,
        Function_Obj,
        String_Obj,
        Builtin_Obj,
        Array_Obj,
        Hash_Obj,
    }
    enum Lowset
    {
        LOWEST = 1,
        EQUALS = 2, // == 
        LESSGREATER = 3, // > or < 
        SUM = 4, // + 
        PRODUCT = 5,// * 
        PREFIX = 6, // -X or !X 
        CALL = 7,
        INDEX = 8
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
        /// 浮点数
        /// </summary>
        Double,
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
        Not_EQ,
        /// <summary>
        /// dot 操作
        /// </summary>
        OP,
        /// <summary>
        /// string
        /// </summary>
        STRING,
        /// <summary>
        /// [
        /// </summary>
        LBRACKET,
        /// <summary>
        /// ]
        /// </summary>
        RBRACKET,
        /// <summary>
        /// :
        /// </summary>
        COLON,

    }
    static class Global
    {
        private class EntryHash : IHashKey
        {
            public MonkeyTypeEnum HashType { get; set; }
            public ulong HashValue { get; set; }

            public IHashKey HashKey()
            {
                return this;
            }
        }
        public static IHashKey Create(this IHashKey hashKey)
        {
            EntryHash hash = new EntryHash() { HashValue = hashKey.HashValue, HashType = hashKey.HashType };
            return hash;
        }
        public delegate IMonkeyobject BuiltinFunction(List<IMonkeyobject> args);
        public static readonly Dictionary<MonkeyTypeEnum, string> MonkeyTypePairs = new Dictionary<MonkeyTypeEnum, string>()
        {
            {MonkeyTypeEnum.Double_Obj, "Double" },
            {MonkeyTypeEnum.Boolean_Obj, "Boolean" },
            {MonkeyTypeEnum.Null_Obj, "Null" },
            {MonkeyTypeEnum.Error_Obj, "Error" },
            {MonkeyTypeEnum.Return_Obj, "Return" },
            {MonkeyTypeEnum.Function_Obj, "Function" },
            {MonkeyTypeEnum.String_Obj, "String" },
            {MonkeyTypeEnum.Builtin_Obj, "Builtin" },
            {MonkeyTypeEnum.Array_Obj, "Array" },
            {MonkeyTypeEnum.Hash_Obj, "Hash" },
        };
        public static readonly Dictionary<TokenEnum, Lowset> Precedences = new Dictionary<TokenEnum, Lowset>()
        {
            { TokenEnum.EQ, Lowset.EQUALS },
            { TokenEnum.Not_EQ, Lowset.EQUALS },
            { TokenEnum.LT, Lowset.LESSGREATER },
            { TokenEnum.GT, Lowset.LESSGREATER },
            { TokenEnum.PLUS, Lowset.SUM },
            { TokenEnum.MINUS, Lowset.SUM },
            { TokenEnum.SLASH, Lowset.PRODUCT },
            { TokenEnum.ASTERISK, Lowset.PRODUCT },
            {TokenEnum.LPAREN,Lowset.CALL },
            {TokenEnum.LBRACKET,Lowset.INDEX }
        };

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
            new Token (TokenEnum.Double,"Double"),
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
