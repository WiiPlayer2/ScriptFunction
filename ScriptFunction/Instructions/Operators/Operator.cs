using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions.Operators
{
    abstract class Operator : Instruction
    {
        protected Operator(int argCount)
        {
            ArgumentCount = argCount;
        }

        public override void Execute(ScriptEnvironment env)
        {
            var args = new object[ArgumentCount];
            for (var i = 0; i < args.Length; i++)
            {
                args[i] = env.Pop();
            }
            env.Push(Operate(args));
        }

        public int ArgumentCount { get; private set; }

        protected abstract object Operate(object[] args);


        protected object Invoke(string methodName, params object[] args)
        {
            var allMethods = args
                .Select(o => o.GetType())
                .SelectMany(o => o.GetMethods())
                .Distinct();
            var methods = GetInvokableMethods(allMethods, methodName, GetTypes(args));

            var method = methods.Single();
            return method.Invoke(null, args);
        }
    }
}
