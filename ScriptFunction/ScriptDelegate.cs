﻿//#define OLD_METHOD

using ScriptFunction.Instructions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptFunction
{
    public class ScriptDelegate
    {
        internal const bool DEFAULT_KEEP_ASYNC = false;

        internal Dictionary<string, int> labelPositions;
        internal Instruction[] instructions;

        internal ScriptDelegate(IEnumerable<Instruction> instructions)
        {
            this.instructions = instructions.ToArray();

            labelPositions = new Dictionary<string, int>();

            var env = new ScriptEnvironment(this);
            for (var i = 0; i < this.instructions.Length; i++)
            {
                env.ProgramCounter = i;
                this.instructions[i].Prepare(env);
            }
        }

        public object DynamicInvoke(bool keepAsync = DEFAULT_KEEP_ASYNC, params object[] args)
        {
#if OLD_METHOD
            var env = new ScriptEnvironment(this, args);
            while (!env.IsReturning
                && env.ProgramCounter < instructions.Length)
            {
                var inst = instructions[env.ProgramCounter];
#if DEBUG
                env.AddHistoryEntry(inst);
#endif
                try
                {
                    inst.Execute(env);
                }
                catch (ScriptException e)
                {
                    HandleException(e, env);
                }
                    catch(TargetInvocationException e)
                {
                    HandleException(new ScriptException(inst, e.InnerException), env);
                }
                catch (Exception e)
                {
                    HandleException(new ScriptException(inst, e), env);
                }
                if (env.ProgramCounter < -1
                    || env.ProgramCounter >= instructions.Length)
                {
                    throw new ScriptException(inst, new IndexOutOfRangeException());
                }
                env.ProgramCounter++;
            }
            return env.ReturnValue;
#else
            var mgr = CreateExecutionManager(args);
            mgr.KeepAsync = keepAsync;
            mgr.ScriptExecutionAdded += (sender, exec) =>
            {
                var thread = new Thread(() =>
                {
                    try
                    {
                        while (exec.Step()) { }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                });
                thread.Start();
            };

            try
            {
                while (mgr.MainExecution.Step()) { }
            }
            catch (Exception e)
            {
                mgr.Quit();
                throw e;
            }
            mgr.Quit();

            return mgr.MainExecution.Environment.ReturnValue;
#endif
        }

        public T DynamicInvoke<T>(bool keepAsync = DEFAULT_KEEP_ASYNC, params object[] args)
        {
            return (T)DynamicInvoke(keepAsync, args);
        }

        public object SyncDynamicInvoke(bool keepAsync = DEFAULT_KEEP_ASYNC, params object[] args)
        {
            var mgr = CreateExecutionManager(args);
            mgr.KeepAsync = keepAsync;

            while (mgr.Step()) { }

            return mgr.MainExecution.Environment.ReturnValue;
        }

        public T SyncDynamicInvoke<T>(bool keepAsync = DEFAULT_KEEP_ASYNC, params object[] args)
        {
            return (T)SyncDynamicInvoke(keepAsync, args);
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

        public ScriptExecutionManager CreateExecutionManager(params object[] args)
        {
            var mgr = new ScriptExecutionManager(this, args);
            return mgr;
        }

        public Delegate AsDelegate()
        {
            return Delegate.CreateDelegate(typeof(ScriptDelegate), this, "DynamicInvoke");
        }

        #region Actions
        public Action AsAction()
        {
            return () => DynamicInvoke();
        }

        public Action<TIn> AsAction<TIn>()
        {
            return a => DynamicInvoke(false, a);
        }

        public Action<T1, T2> AsAction<T1, T2>()
        {
            return (a, b) => DynamicInvoke(false, a, b);
        }
        #endregion

        #region Funcs
        public Func<TOut> AsFunc<TOut>()
        {
            return () => DynamicInvoke<TOut>();
        }

        public Func<TIn, TOut> AsFunc<TIn, TOut>()
        {
            return a => DynamicInvoke<TOut>(false, a);
        }

        public Func<T1, T2, TOut> AsFunc<T1, T2, TOut>()
        {
            return (a, b) => DynamicInvoke<TOut>(false, a, b);
        }
        #endregion
    }
}
