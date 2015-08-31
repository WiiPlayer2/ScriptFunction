using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class LoadArgument : Instruction
    {
        public LoadArgument(int arg)
        {
            Argument = arg;
        }

        public int Argument { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            env.Push(env[Argument]);
        }
    }
}
