using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    internal class Test
    {
        public static void TestLexer()
        {
            var x = "let five = 5.523; " +
                "let ten = 10;" +
                "let add = fn(x, y) " +
                "{x + y;};" +
                "let result = add(five, ten);" +
                "!-/*5;5 < .10 > 0.55;" +
                "if (5 < 10){return true;} " +
                "else{return false;}" +
                "10 == 10.254;" +
                "10 != 9; ";
            var m = Lexer.Create(x);
            List<Token> tokens = new List<Token>();
            var tok = m.NextToken();
            tok.Console();
            while (tok.TokenEnum != TokenEnum.EOF)
            {
                tok = m.NextToken();
                tok.Console();
                tokens.Add(tok);
            }
        }
        public static void LetStatement()
        {
            var input = " " +
                "let x = 5;" +
                "let y = 10;" +
                "let foobar = 838383; ";
            var lexer = Lexer.Create(input);
            var p = Parser.Create(lexer);
            var list = p.ParserProgram();
        }
        public static void ReturnStatement()
        {
            var input = " " +
                "return 5;" +
                "return 10;" +
                "return 838383 33; ";
            var lexer = Lexer.Create(input);
            var p = Parser.Create(lexer);
            var list = p.ParserProgram();
            foreach (var item in list.Statement)
            {
                var x = item.TokenLiteral();
            }
        }
        public static void Expression()
        {
            var input = "foobar";
            var lexer = Lexer.Create(input);
            var p = Parser.Create(lexer);
            var list = p.ParserProgram();
        }
        public static void Number()
        {
            var input = "5.6785";
            var lexer = Lexer.Create(input);
            var p = Parser.Create(lexer);
            var list = p.ParserProgram();
        }
        public static void Prefix()
        {
            var input = "-5;" +
                "!foobar; " +
                "5   -10;";
            var lexer = Lexer.Create(input);
            var p = Parser.Create(lexer);
            var list = p.ParserProgram();
        }
        public static void Infix()
        {
            //var input = "5 + 5; 5 - 5;5 * 5;5 / 5;5 > 5;5 == 5;";
            //var input = new string[] { "-a * b ", "!-a" ,"a + b + c" ,"3 + 4; -5 * 5"};
            var input = new string[] { "-1+2", };
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                Console.WriteLine(list.OutLine());
            }
        }
        public static void IFELSE()
        {
            var input = new string[] { "if (x < y) { x } else { y } ", };
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                Console.WriteLine(list.OutLine());
            }
        }
        public static void FN()
        {
            var input = new string[] { "fn(x) {};", "fn(x,y,z) {};", };
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                Console.WriteLine((list.Statement[0] as ExpressionStatement).Expression.OutLine());

            }
        }
        public static void FN2()
        {
            //let x = 1 * 2 * 3 * 4 * 5 
            var input = new string[] { "let x = mmm12 * 3; " };
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                Console.WriteLine(list.OutLine());
            }
        }
        public static void EvalDouble()
        {
            var input = new string[] { "5 ", "99.36", "true", "false" };
            foreach (var item in input)
            {

                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                var eval = Eval.Create().InitEval(list);
                Console.WriteLine((eval as MonkeyDouble).Value);

            }
        }

        public static void EvalBoolean()
        {
            var input = new string[] { "5 ", "99.36", "true", "false" };

            EvalTest(input);
        }
        public static void EvalDoubles()
        {
            var input = new string[] { "-5 ", "!!99.36", "!true", "false","-true" };

            EvalTest(input);
        }
        public static void EvalTest(string[] input)
        {
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                var eval = Eval.Create().InitEval(list);
                Monkeyobject output = eval;
                Console.WriteLine(output.Inspect());
            }
        }
        public static void Start()
        {
            EvalDoubles();
        }
    }
}
