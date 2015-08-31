using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Call : Instruction
    {
        public Call(string label)
        {
            Label = label;
        }

        public string Label { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            env.CallPush(env.ProgramCounter);
            env.JumpToLabel(Label);
        }
    }
}
