using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Swap : Instruction
    {
        public override void Execute(ScriptEnvironment env)
        {
            var v1 = env.Pop();
            var v2 = env.Pop();

            env.Push(v1);
            env.Push(v2);
        }
    }
}
