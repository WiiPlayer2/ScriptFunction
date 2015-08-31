using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class LoadVariable : Instruction
    {
        public LoadVariable(string var)
        {
            Variable = var;
        }

        public string Variable { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            env.Push(env[Variable]);
        }
    }
}
