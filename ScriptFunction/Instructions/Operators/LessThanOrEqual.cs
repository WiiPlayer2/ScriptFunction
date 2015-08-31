using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions.Operators
{
    class LessThanOrEqual : BinaryOperator
    {
        protected override object PrimitiveOperate(Type type, object arg1, object arg2)
        {
            if (type == typeof(double))
            {
                return (double)arg1 <= (double)arg2;
            }
            if (type == typeof(int))
            {
                return (int)arg1 <= (int)arg2;
            }
            throw new NotImplementedException();
        }

        protected override object Operate(object arg1, object arg2)
        {
            return Invoke("op_LessThanOrEqual", arg1, arg2);
        }

        protected override IEnumerable<Type> PrimitiveTypes
        {
            get
            {
                return new[]
                {
                    typeof(int),
                    typeof(double),
                };
            }
        }
    }
}
