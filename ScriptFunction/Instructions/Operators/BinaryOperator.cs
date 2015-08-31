using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions.Operators
{
    abstract class BinaryOperator : Operator
    {
        protected BinaryOperator()
            : base(2) { }

        protected override object Operate(object[] args)
        {
            return PreOperate(args[0], args[1]);
        }

        protected abstract object PrimitiveOperate(Type type, object arg1, object arg2);

        protected abstract object Operate(object arg1, object arg2);
        private object PreOperate(object arg1, object arg2)
        {
            var t1 = arg1.GetType();
            var t2 = arg2.GetType();
            if(t1.IsPrimitive 
                && t2.IsPrimitive)
            {
                var res = default(object);
                PrimitiveTypes.First(o => Check(o, arg1, arg2, out res));
                return res;
            }
            else
            {
                return Operate(arg1, arg2);
            }
        }

        protected abstract IEnumerable<Type> PrimitiveTypes { get; }

        private bool Check(Type type, object arg1, object arg2, out object result)
        {
            result = null;
            var t1 = arg1.GetType();
            var t2 = arg2.GetType();

            if (t1 == type || t2 == type)
            {
                try
                {
                    result = PrimitiveOperate(type, arg1, arg2);
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
