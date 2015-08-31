using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class LoadConstant<T> : Instruction
    {
        public LoadConstant(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            env.Push(Value);
        }
    }
}
