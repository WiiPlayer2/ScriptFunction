using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Branch : Instruction
    {
        public Branch(string name)
        {
            Name = name;
        }

        public override void Execute(ScriptEnvironment env)
        {
            var val = env.Pop();
            if((bool)val)
            {
                env.JumpToLabel(Name);
            }
        }

        public string Name { get; private set; }
    }
}
