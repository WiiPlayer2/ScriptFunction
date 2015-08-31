using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class TypeOf : Instruction
    {
        public TypeOf(string typeName)
        {
            TypeName = typeName;
            IsIntermediate = true;
        }

        public TypeOf()
        {
            IsIntermediate = false;
        }

        public bool IsIntermediate { get; private set; }

        public string TypeName { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            var name = TypeName;
            if(!IsIntermediate)
            {
                name = (string)env.Pop();
            }

            var type = GetType(name);
            env.Push(type);
        }

        public static Type GetType(string typeName)
        {
            return Type.GetType(typeName, true);
        }
    }
}
