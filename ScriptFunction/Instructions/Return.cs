using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Return : Instruction
    {
        public Return(bool isVoid)
        {
            IsVoid = isVoid;
        }

        public override void Execute(ScriptEnvironment env)
        {
            if(!IsVoid)
            {
                env.ReturnValue = env.Pop();
            }
            env.IsReturning = true;
        }

        public bool IsVoid { get; private set; }
    }
}
