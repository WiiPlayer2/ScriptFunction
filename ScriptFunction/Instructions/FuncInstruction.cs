using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    public class FuncInstruction : Instruction
    {
        private Func<ScriptEnvironment, object> func;

        public FuncInstruction(Func<ScriptEnvironment, object> action)
        {
            this.func = action;
        }

        public override void Execute(ScriptEnvironment env)
        {
            env.Push(func(env));
        }
    }
}
