using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public struct MArg
    {
        public string Name { get; set; }

        public string ValueRegex { get; set; }

        public string Regex
        {
            get
            {
                return string.Format("(?<{0}>{1})", Name, ValueRegex);
            }
        }

        private static MArg Arg(string name, string valueRegex)
        {
            return new MArg()
            {
                Name = name,
                ValueRegex = valueRegex,
            };
        }

        public static IEnumerable<MArg> Arg(params MArg[] args)
        {
            return args;
        }

        public static MArg Integer(string name)
        {
            return Arg(name, @"\-?\d+");
        }

        public static MArg String(string name)
        {
            return Arg(name, @"\"".*\""");
        }

        public static MArg Member(string name)
        {
            return Arg(name, @"(\w|\d|\.|_|\,)+");
        }

        public static MArg Label(string name)
        {
            return Arg(name, @"\w+");
        }

        public static MArg Variable(string name)
        {
            return Arg(name, @"(\w|\d)+");
        }
    }
}
