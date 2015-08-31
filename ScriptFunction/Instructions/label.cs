using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Label : Instruction
    {
        public Label(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override void Prepare(ScriptEnvironment env)
        {
            env.RegisterLabel(Name);
        }

        public override void Execute(ScriptEnvironment env) { }
    }
}
