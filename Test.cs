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
                var eval = Evaluator.Create().Eval(list, MonkeyEnvironment.Create());
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
            var input = new string[] { "-5 ", "!!99.36", "!true", "false", "-true" };

            EvalTest(input);
        }
        public static void EvalDoublesMid()
        {
            var input = new string[] { " 500 / 2 != 250 ", "3 * (3 * 3.6) + 10", "5==6", "1==2", "3!=3", "2==2", "3 + 4 * 5 == 3 * 1 + 4 * 5" };

            EvalTest(input);
        }
        public static void EvalIf()
        {
            var input = new string[]
            {
                " if (5 * 5 + 10 > 34) { 99 } else { 100 } ",
                "if (false){10}"
            };
            EvalTest(Tuple.Create(input, new List<IMonkeyobject> { new MonkeyDouble() { Value = 99 }, new MonkeyNull() }));
        }
        public static void EvalReturn()
        {
            var input = new string[]
            {
                "if (10 > 1){if (10 > 1){return 10;}return 1;}"

            };
            EvalTest(Tuple.Create(input, new List<IMonkeyobject> { new MonkeyDouble() { Value = 10 } }));
        }
        public static void EvalError()
        {
            //"true + false;
            var input = new string[]
           {
                "true + false;"

           };
            EvalTest(input);
        }
        public static void Evalid()
        {
            //"true + false;
            var input = new string[]
           {
                "let id=5;" +
                "if(id==5)" +
                "{ " +
                "let id=99; " +
                "return 66;" +
                "}" +
                "else{" +
                "return true;" +
                "}"

           };
            EvalTest(input);
        }
        public static void Evalfunc()
        {
            //"true + false;
            var input = new string[]
           {
                "let double = fn(x) { x * 2; }; double(5);",
                " let newAdder = fn(x) { fn(y) { x + y } } let addTwo = newAdder(6.6); addTwo(3);",
                "let x=\"This is amazing!\";let xo=fn(u){return x+u;} xo(\"kkk\");",
                "let x=\"a\";let b=\"a\" ;return a!=b;",
                "len(\"abc\")",
                "let x=fn(i){return len(i)*5;} x(\"jjxj\")"
           };
            EvalTest(input);
        }
        public static void EvalArray()
        {
            //"a * [1, 2, 3, 4][b * c] * d",
            var input = new string[]
         {
               "a * [1, 2, 3, 4][b * c] * d"
               ,
               "((a * ([1, 2, 3, 4][(b * c)])) * d)"
         };
            EvalTestParser(input);
        }
        public static void EvalArray2()
        {
            //"a * [1, 2, 3, 4][b * c] * d",
            var input = new string[]
            {
               "[1, 2, 3, 4]",
               "let double = fn(x) { x * 2 };" +
               "[1, double(2), 3 * 3, 4 - 3] "
            };
            EvalTest(input);
        }
        public static void EvalArray3()
        {
            //"a * [1, 2, 3, 4][b * c] * d",
            var input = new string[]
            {
               "let a = [1, 2 * 2, 10 - 5, 8 / 2]; a[99] ",
              // "let double = fn(x) { x * 2 };" +
              // "[1, double(2), 3 * 3, 4 - 3] "
            };
            EvalTest(input);
        }
        public static void EvalArray4()
        {
            //"a * [1, 2, 3, 4][b * c] * d",
            var input = new string[]
            {
               "let a = [1, 2 * 2, 10 - 5, 8 / 2]; push(a,999);",
              // "let double = fn(x) { x * 2 };" +
              // "[1, double(2), 3 * 3, 4 - 3] "
            };
            EvalTest(input);
        }
        // let people = [{"name": "Alice", "age": 24}, {"name": "Anna", "age": 28}]; 
        public static void EvalHash()
        {
            //"a * [1, 2, 3, 4][b * c] * d",
            var input = new string[]
            {
               "let people = [{\"name\": \"Alice\", \"age\": 24}, {\"name\": \"Anna\", \"age\": 28}];people[0][\"name\"]; ",
              // "let double = fn(x) { x * 2 };" +
              // "[1, double(2), 3 * 3, 4 - 3] "
            };
            EvalTest(input);
        }
        public static void EvalTestParser(string[] input)
        {
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                Console.WriteLine(list.OutLine());
            }
        }
        public static void EvalTest(string[] input)
        {
            foreach (var item in input)
            {
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                var eval = Evaluator.Create().Eval(list, MonkeyEnvironment.Create());
                IMonkeyobject output = eval;
                Console.WriteLine(output.Inspect());
            }
        }
        public static void EvalTest(Tuple<string[], List<IMonkeyobject>> input)
        {
            for (int i = 0; i < input.Item1.Length; i++)
            {
                var item = input.Item1[i];
                var lexer = Lexer.Create(item);
                var p = Parser.Create(lexer);
                var list = p.ParserProgram();
                var eval = Evaluator.Create().Eval(list, MonkeyEnvironment.Create());
                IMonkeyobject output = eval;
                if (output != null)
                {
                    if (output.Inspect() == input.Item2[i].Inspect())
                    {
                        Console.WriteLine($"output eq  value is {output.Inspect()}");
                    }
                    else
                    {
                        Console.WriteLine($"output not eq  value is {output.Inspect()}");
                    }

                }
            }
        }
        public static void Start()
        {
            EvalHash();
        }
    }
}
