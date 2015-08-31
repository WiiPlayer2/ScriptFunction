using ScriptFunction.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class ScriptException : Exception
    {
        internal ScriptException(Instruction instruction, Exception innerException)
            : base(string.Format("{0} @ Line {1}: \"{2}\"\n{3}", innerException.GetType(),
                instruction.LineNumber, instruction.CodeLine, innerException.Message),
                innerException) { }

        internal ScriptEnvironment Environment { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n\n{1}", base.ToString(), ScriptStackTrace);
        }

        public string ScriptStackTrace
        {
            get
            {
                return string.Join("\n", new int[] { Environment.ProgramCounter }
                    .Concat(Environment.callStack.Cast<int>())
                    .Select(o => Environment.Delegate.instructions[o])
                    .Select(o => string.Format("   at {0}", o)));
            }
        }
    }
}
