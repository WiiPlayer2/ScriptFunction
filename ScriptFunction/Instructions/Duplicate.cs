using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Duplicate : Instruction
    {
        public override void Execute(ScriptEnvironment env)
        {
            var obj = env.Pop();
            env.Push(obj);
            env.Push(obj);
        }
    }
}
