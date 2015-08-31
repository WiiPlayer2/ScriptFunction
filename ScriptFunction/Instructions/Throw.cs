using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Throw : Instruction
    {
        public override void Execute(ScriptEnvironment env)
        {
            var excpetion = (Exception)env.Pop();
            throw excpetion;
        }
    }
}
