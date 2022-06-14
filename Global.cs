using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
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
