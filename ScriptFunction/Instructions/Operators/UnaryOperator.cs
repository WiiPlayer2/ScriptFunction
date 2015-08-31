using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions.Operators
{
    abstract class UnaryOperator : Operator
    {
        protected UnaryOperator()
            : base(1) { }

        protected override object Operate(object[] args)
        {
            return Operate(args[0]);
        }

        protected abstract object Operate(object arg);
    }
}
