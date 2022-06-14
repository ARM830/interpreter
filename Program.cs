using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
   
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
