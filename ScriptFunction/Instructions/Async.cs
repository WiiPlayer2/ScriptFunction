using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction.Instructions
{
    class Async : Instruction
    {
        public Async(string label, int argCount)
        {
            ArgumentCount = argCount;
            Label = label;
        }

        public string Label { get; private set; }

        public int ArgumentCount { get; private set; }

        public override void Execute(ScriptEnvironment env)
        {
            var newEnv = env.Split();

            var args = new object[ArgumentCount];
            for (var i = 0; i < ArgumentCount; i++)
            {
                args[i] = env.Pop();
            }
            for(var i = ArgumentCount - 1; i >= 0; i--)
            {
                newEnv.Push(args[i]);
            }

            newEnv.JumpToLabel(Label);
            newEnv.ProgramCounter++;
            var exec = new ScriptExecution(newEnv);
            env.ExecutionManager.Add(exec);
        }
    }
}
