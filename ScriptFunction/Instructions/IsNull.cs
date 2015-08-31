using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class IsNull : Instruction
    {
        public IsNull(bool peek)
        {
            Peek = peek;
        }

        public bool Peek { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            var obj = env.Pop();
            if(Peek)
            {
                env.Push(obj);
            }

            env.Push(obj == null);
        }
    }
}
