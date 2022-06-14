using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
   
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
