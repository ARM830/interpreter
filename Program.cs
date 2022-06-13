using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{

    enum TokenKey
    {
        ILLEGAL,
        EOF,
        // 标识符+字面量
        IDENT,// add, foobar, x, y, ... 
        INT,// 1343456 
            // 运算符
        ASSIGN,
        PLUS,
        // 分隔符
        COMMA,
        SEMICOLON,
        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,
        // 关键字
        FUNCTION,
        LET,
    }
    class Token
    {
        public TokenKey TokenType { get; set; }
        public string Literal { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
