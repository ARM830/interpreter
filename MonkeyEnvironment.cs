using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解释器
{
    internal class MonkeyEnvironment
    {
        Dictionary<string, IMonkeyobject> Store { get; set; }

        static MonkeyEnvironment()
        {

        }
        private MonkeyEnvironment()
        {
            Store = new Dictionary<string, IMonkeyobject>();
        }
        public static MonkeyEnvironment Create()
        {
            return new MonkeyEnvironment();
        }
        public Tuple<IMonkeyobject, bool> Get(string name)
        {
            IMonkeyobject obj = null;
            if (Store.TryGetValue(name, out obj))
            {
                return new Tuple<IMonkeyobject, bool>(obj, true);
            }
            return new Tuple<IMonkeyobject, bool>(obj, false);
        }
        public IMonkeyobject Set(string name, IMonkeyobject monkeyobject)
        {
            Store[name] = monkeyobject;
            return monkeyobject;
        }
    }
}
