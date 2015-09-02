using ScriptFunction.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class ScriptEnvironment
    {
        private object[] args;
        private Dictionary<string, object> vars;
        private Stack<object> stack;
        internal Stack<object> callStack;

#if DEBUG
        private List<Instruction> execHistory;
        internal object lastPoppedObject;
#endif

        internal ScriptEnvironment(ScriptDelegate sdelegate)
        {
            Delegate = sdelegate;
        }

        internal ScriptEnvironment(ScriptDelegate sdelegate, object[] args)
            : this(sdelegate)
        {
            this.args = args;

            vars = new Dictionary<string, object>();
            stack = new Stack<object>();
            callStack = new Stack<object>();

#if DEBUG
            execHistory = new List<Instruction>();
#endif
        }

#if DEBUG
        internal void AddHistoryEntry(Instruction inst)
        {
            execHistory.Add(inst);
        }
#endif

        public int ProgramCounter { get; set; }

        public object ReturnValue { get; set; }

        public bool IsReturning { get; set; }

        public void RegisterLabel(string label)
        {
            Delegate.labelPositions[label] = ProgramCounter;
        }

        public void JumpToLabel(string label)
        {
            ProgramCounter = Delegate.labelPositions[label] - 1;
        }

        public object this[int argIndex]
        {
            get
            {
                return args[argIndex];
            }
        }

        public object this[string varName]
        {
            get
            {
                return vars[varName];
            }
            set
            {
                vars[varName] = value;
            }
        }

        public void Push(object obj)
        {
            stack.Push(obj);
        }

        public object Pop()
        {
            var obj = stack.Pop();
#if DEBUG
            lastPoppedObject = obj;
#endif
            return obj;
        }

        public object Peek()
        {
            return stack.Peek();
        }

        public void CallPush(object obj)
        {
            callStack.Push(obj);
        }

        public object CallPop()
        {
            return callStack.Pop();
        }

        public object CallPeek()
        {
            return callStack.Peek();
        }

        public ScriptExecutionManager ExecutionManager { get; internal set; }

        public ScriptDelegate Delegate { get; private set; }

        public string TryLabel { get; set; }

        public object SpecialArgument { get; set; }

        public T GetSpecialArgument<T>()
        {
            return (T)SpecialArgument;
        }

        public ScriptEnvironment Split()
        {
            var env = new ScriptEnvironment(Delegate, args);
            env.ExecutionManager = ExecutionManager;
            env.vars = vars;
            env.ProgramCounter = ProgramCounter;
            return env;
        }
    }
}
