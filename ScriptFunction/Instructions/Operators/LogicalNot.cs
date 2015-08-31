using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions.Operators
{
    class LogicalNot : UnaryOperator
    {
        protected override object Operate(object arg)
        {
            var t = arg.GetType();

            if(t.IsPrimitive)
            {
                if(arg is bool)
                {
                    return !((bool)arg);
                }
                throw new NotImplementedException();
            }
            else
            {
                return Invoke("op_LogicalNot", arg);
            }
        }
    }
}
