using ScriptFunction.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class DotNetScriptParser : ScriptParser
    {
        static DotNetScriptParser()
        {
            #region Commands
            RegisterCommand("typeof.i", @"(?<type>(\w|\d|\.|_)+)", TypeOfIntermediate);
            RegisterCommand("typeof", m => new TypeOf());
            RegisterCommand("invoke.si", @"(?<name>(\w|\d|\.|_)+)\,(?<arg>\d+)", InvokeStaticIntermediate);
            RegisterCommand("invoke.vi", @"(?<name>(\w|\d|\.|_)+)\,(?<arg>\d+)", InvokeVirtualIntermediate);
            RegisterCommand("invoke.s", m => new Invoke(true));
            RegisterCommand("invoke.v", m => new Invoke(false));
            RegisterCommand("newobj", m => new NewObject(true));
            RegisterCommand("newobj.s", m => new NewObject(false));
            RegisterCommand("newobj.i", @"(?<type>(\w|\d|\.|_)+)\,(?<arg>\d+)", NewObjectIntermediate);
            RegisterCommand("throw", m => new Throw());
            #endregion

            #region Macros
            RegisterMacro("print", "typeof.i System.Console\ninvoke.si Write,1");
            RegisterMacro("println", "typeof.i System.Console\ninvoke.si WriteLine,1");
            RegisterMacro("print",
                MArg.Arg(MArg.String("txt")),
                "ld.str $txt$\ntypeof.i System.Console\ninvoke.si Write,1");
            RegisterMacro("println",
                MArg.Arg(MArg.String("txt")),
                "ld.str $txt$\ntypeof.i System.Console\ninvoke.si WriteLine,1");

            RegisterMacro("get",
                MArg.Arg(MArg.Member("prop")),
                "invoke.vi get_$prop$,0");
            RegisterMacro("set",
                MArg.Arg(MArg.Member("prop")),
                "invoke.vi set_$prop$,1");

#if DEBUG
            RegisterMacro("break", "typeof.i System.Diagnostics.Debugger\ninvoke.si Break,0");
#else
            RegisterMacro("break", "nop");
#endif
            #endregion
        }

        private static Instruction TypeOfIntermediate(Match arg)
        {
            var type = arg.Groups["type"].Value;
            return new TypeOf(type);
        }

        private static Instruction NewObjectIntermediate(Match arg)
        {
            var type = arg.Groups["type"].Value;
            var args = int.Parse(arg.Groups["arg"].Value);
            return new NewObject(type, args);
        }

        private static Instruction InvokeVirtualIntermediate(Match arg)
        {
            var name = arg.Groups["name"].Value;
            var args = int.Parse(arg.Groups["arg"].Value);
            return new Invoke(name, args, false);
        }

        private static Instruction InvokeStaticIntermediate(Match arg)
        {
            var name = arg.Groups["name"].Value;
            var args = int.Parse(arg.Groups["arg"].Value);
            return new Invoke(name, args, true);
        }
    }
}
