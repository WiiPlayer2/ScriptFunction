using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class NewObject : Instruction
    {
        public NewObject(string type, int argCount)
        {
            TypeName = type;
            ArgumentCount = argCount;
            IsIntermediate = true;
        }

        public NewObject(bool useDefaultConstructor)
        {
            IsIntermediate = false;
            UseDefaulConstructor = useDefaultConstructor;
        }

        public string TypeName { get; private set; }

        public int ArgumentCount { get; private set; }

        public bool IsIntermediate { get; private set; }

        public bool UseDefaulConstructor { get; set; }

        public override void Execute(ScriptEnvironment env)
        {
            Type type = null;
            if(!IsIntermediate)
            {
                if (!UseDefaulConstructor)
                {
                    ArgumentCount = (int)env.Pop();
                }
                type = (Type)env.Pop();
            }
            else
            {
                type = TypeOf.GetType(TypeName);
            }

            var args = new object[ArgumentCount];
            for (var i = 0; i < args.Length; i++)
            {
                args[i] = env.Pop();
            }

            var obj = Activator.CreateInstance(type, args);
            env.Push(obj);
        }
    }
}
