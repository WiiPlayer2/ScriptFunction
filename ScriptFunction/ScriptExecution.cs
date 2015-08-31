using ScriptFunction.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class ScriptExecution
    {
        private bool quit;

        internal ScriptExecution(ScriptEnvironment env)
        {
            Environment = env;
        }

        public ScriptEnvironment Environment { get; private set; }

        public bool Step()
        {
            if(quit)
            {
                return false;
            }

            CurrentInstruction = Environment.Delegate.instructions[Environment.ProgramCounter];

#if DEBUG
            Environment.AddHistoryEntry(CurrentInstruction);
#endif

            try
            {
                CurrentInstruction.Execute(Environment);
            }
            catch(ScriptException e)
            {
                HandleException(e, Environment);
            }
            catch(TargetInvocationException e)
            {
                HandleException(new ScriptException(CurrentInstruction, e.InnerException), Environment);
            }
            catch(Exception e)
            {
                HandleException(new ScriptException(CurrentInstruction, e), Environment);
            }

            if (Environment.ProgramCounter < -1
                || Environment.ProgramCounter >= Environment.Delegate.instructions.Length)
            {
                throw new ScriptException(CurrentInstruction, new IndexOutOfRangeException());
            }

            Environment.ProgramCounter++;

            if(Environment.IsReturning
                || Environment.ProgramCounter >= Environment.Delegate.instructions.Length)
            {
                quit = true;
                return false;
            }
            return true;
        }

        private void HandleException(ScriptException e, ScriptEnvironment env)
        {
            if (env.TryLabel == null)
            {
                e.Environment = env;
                throw e;
            }

            env.Push(e.InnerException);
            env.JumpToLabel(env.TryLabel);
            env.TryLabel = null;
        }

        public void Quit()
        {
            quit = true;
        }

        public Instruction CurrentInstruction { get; private set; }
    }
}
