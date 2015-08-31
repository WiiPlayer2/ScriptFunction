using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Try : Instruction
    {
        public Try(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            env.TryLabel = Name;
        }
    }
}
