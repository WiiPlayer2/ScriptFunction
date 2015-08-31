using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Invoke : Instruction
    {
        public Invoke(string method, int argCount, bool isStatic)
        {
            Method = method;
            ArgumentCount = argCount;
            IsStatic = isStatic;
            IsIntermediate = true;
        }

        public Invoke(bool isStatic)
        {
            IsStatic = isStatic;
            IsIntermediate = false;
        }

        public string Method { get; private set; }

        public int ArgumentCount { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsIntermediate { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            Object obj = null;
            Type type = null;

            if(!IsIntermediate)
            {
                ArgumentCount = (int)env.Pop();
                Method = (string)env.Pop();
            }

            if (IsStatic)
            {
                type = (Type)env.Pop();
            }
            else
            {
                obj = env.Pop();
                type = obj.GetType();
            }
            var args = new object[ArgumentCount];
            for (var i = 0; i < args.Length; i++)
            {
                args[i] = env.Pop();
            }
            var allMethods = type.GetMethods();
            var methods = GetInvokableMethods(allMethods, Method, GetTypes(args));
            var method = methods.First();
            var ret = method.Invoke(obj, args);
            if (method.ReturnType != typeof(void))
            {
                env.Push(ret);
            }
        }
    }
}
