using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class ReturnCall:Instruction
    {
        public override void Execute(ScriptEnvironment env)
        {
            var pc = (int)env.CallPop();
            env.ProgramCounter = pc;
        }
    }
}
