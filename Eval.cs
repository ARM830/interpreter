using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    interface Monkeyobject
    {
        string MonkeyObjectType();
        string Inspect();
    }
    class MonkeyDouble : Monkeyobject
    {
        public double Value { get; set; }
        public string Inspect()
        {
            return Value.ToString();
        }

        public string MonkeyObjectType()
        {
            return MonkeyTypeEnum.Double_Obj.ToString();
        }
    }
    class MonkeyBoolean : Monkeyobject
    {
        public bool Value { get; set; }
        public string Inspect()
        {
            return Value.ToString();
        }

        public string MonkeyObjectType()
        {
            return MonkeyTypeEnum.Boolean_Obj.ToString();
        }
    }
    class MonkeyNull : Monkeyobject
    {
        public string Inspect()
        {
            return "Null";
        }

        public string MonkeyObjectType()
        {
            return MonkeyTypeEnum.Null_Obj.ToString();
        }
    }
    internal class Eval
    {
        public static Eval Create()
        {
            Eval eval = new Eval();
            return eval;
        }
        static Eval()
        {

        }
        private Eval()
        {

        }
    }
}
