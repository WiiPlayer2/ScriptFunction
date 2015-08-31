using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class ScriptExecutionManager : IEnumerable<ScriptExecution>
    {
        private Queue<ScriptExecution> executions;

        internal ScriptExecutionManager(ScriptDelegate deleg, object[] args)
        {
            Delegate = deleg;

            executions = new Queue<ScriptExecution>();
            var env = new ScriptEnvironment(Delegate, args);
            env.ExecutionManager = this;
            MainExecution = new ScriptExecution(env);
            executions.Enqueue(MainExecution);
        }

        public ScriptDelegate Delegate { get; private set; }

        public ScriptExecution MainExecution { get; private set; }

        public void Add(ScriptExecution execution)
        {
            lock (executions)
            {
                if (!executions.Contains(execution))
                {
                    executions.Enqueue(execution);
                    if (ScriptExecutionAdded != null)
                    {
                        ScriptExecutionAdded(this, execution);
                    }
                }
            }
        }

        public void Remove(ScriptExecution execution)
        {
            lock (executions)
            {
                if (executions.Contains(execution))
                {
                    var exec = executions.Dequeue();
                    while(exec != execution)
                    {
                        executions.Enqueue(exec);
                        exec = executions.Dequeue();
                    }
                }
            }
        }

        public bool Step()
        {
            lock (executions)
            {
                var exec = executions.Dequeue();

                var res = false;
                try
                {
                    res = exec.Step();
                }
                catch (ScriptException e)
                {
                    if (exec == MainExecution)
                    {
                        throw e;
                    }
                }
                catch (Exception e)
                {
                    if (exec == MainExecution)
                    {
                        throw new ScriptException(exec.CurrentInstruction, e);
                    }
                }

                if (!res)
                {
                    if (exec == MainExecution)
                    {
                        Quit();
                        return false;
                    }
                }
                else
                {
                    executions.Enqueue(exec);
                }
                return true;
            }
        }

        public void Quit()
        {
            lock (executions)
            {
                foreach (var e in executions)
                {
                    e.Quit();
                }
            }
        }

        public delegate void ScriptExecutionAddedEventHandler(ScriptExecutionManager sender, ScriptExecution execution);
        public event ScriptExecutionAddedEventHandler ScriptExecutionAdded;

        public IEnumerator<ScriptExecution> GetEnumerator()
        {
            return executions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
