using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    public class ActionInstruction : Instruction
    {
        private Action<ScriptEnvironment> action;

        public ActionInstruction(Action<ScriptEnvironment> action)
        {
            this.action = action;
        }

        public override void Execute(ScriptEnvironment env)
        {
            action(env);
        }
    }
}
