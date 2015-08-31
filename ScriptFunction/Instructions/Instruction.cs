using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    public abstract class Instruction
    {
        public virtual void Prepare(ScriptEnvironment env) { }

        public abstract void Execute(ScriptEnvironment env);

        public string CodeLine { get; internal set; }

        public int LineNumber { get; internal set; }
        protected IEnumerable<MethodInfo> GetInvokableMethods(IEnumerable<MethodInfo> methods,
            string methodName, IEnumerable<Type> argTypes)
        {
            var invokMethods = GetInvokableMethods(methods, methodName, argTypes.Count());
            var ret = invokMethods
                .Where(o =>
                {
                    var param = o.GetParameters();
                    var zip = param
                        .Zip(argTypes, (p, a) =>
                        {
                            var ret3 = p.ParameterType.IsAssignableFrom(a)
                                || !p.ParameterType.IsValueType && a == typeof(NullType)
                                || p.ParameterType.Name == "Nullable`1";
                            return ret3;
                        });
                    var ret2 = zip.Aggregate(true, (acc, curr) => acc && curr);
                    return ret2;
                });
            return ret;
        }

        private IEnumerable<MethodInfo> GetInvokableMethods(IEnumerable<MethodInfo> methods,
            string methodName, int argCount)
        {
            var ret = methods
                .Where(o => o.Name == methodName && o.GetParameters().Length == argCount);
            return ret;
        }

        protected IEnumerable<Type> GetTypes(params object[] args)
        {
            return args.Select(o => o == null ? typeof(NullType) : o.GetType());
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", LineNumber, CodeLine);
        }

        protected static class NullType { }
    }
}
