using ScriptFunction.Instructions;
using ScriptFunction.Instructions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class ScriptParser : BasicScriptParser
    {
        static ScriptParser()
        {
            #region Commands
            RegisterCommand("ld.c", @"(?<val>\-?\d+)", LoadInt);
            RegisterCommand("ld.b", @"(?<val>([Tt][Rr][Uu][Ee]|[Ff][Aa][Ll][Ss][Ee]))", LoadBoolean);
            RegisterCommand("ld.str", @"\""(?<str>.*)\""", LoadString);
            RegisterCommand("op", @"(?<op>(\+|\-|\*|\/|\!|\<\=))", Operator);
            RegisterCommand("null", m => new FuncInstruction(env => null));
            RegisterCommand("isnull", m => new IsNull(false));
            RegisterCommand("isnull.p", m => new IsNull(true));
            #endregion

            #region Macros
            RegisterMacro("incr", "ld.c 1\nop +");
            RegisterMacro("decr", "ld.c 1\nop -");
            #endregion
        }

        private static Instruction LoadBoolean(Match arg)
        {
            var val = bool.Parse(arg.Groups["val"].Value);
            return new LoadConstant<bool>(val);
        }

        private static Instruction LoadString(Match arg)
        {
            var val = arg.Groups["str"].Value;
            return new LoadConstant<string>(val);
        }

        private static Instruction LoadInt(Match arg)
        {
            var val = int.Parse(arg.Groups["val"].Value);
            return new LoadConstant<int>(val);
        }

        private static Instruction Operator(Match arg)
        {
            var op = arg.Groups["op"].Value;
            switch (op)
            {
                case "!":
                    return new LogicalNot();
                case "+":
                    return new Addition();
                case "<=":
                    return new LessThanOrEqual();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
