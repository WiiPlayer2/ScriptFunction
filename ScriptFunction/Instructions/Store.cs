using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Store : Instruction
    {
        public Store(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            env[Name] = env.Pop();
        }
    }
}
