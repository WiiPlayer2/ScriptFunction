using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class TryNot : Instruction
    {
        public override void Execute(ScriptEnvironment env)
        {
            env.TryLabel = null;
        }
    }
}
