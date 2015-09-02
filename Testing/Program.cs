using ScriptFunction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        private static ScriptParser parser;

        static void Main(string[] args)
        {
            parser = new DotNetScriptParser();
            parser.RegisterUserMacro("hello", "ld.str \"Hello World!\"\nprintln");
            parser.RegisterUserMacro("hello2", "hello\nhello\nld.str \"Hello!\"\nprintln\nhello");

            var methods = typeof(Tmp).GetMethods();

            //var ret = parser.Invoke(File.ReadAllText(@"R:\dlsystem-scripts\testing.code"));

            var exec = parser.Create(@"jmp start
lbl async
println
println

ld.b false
sto wait
ret

lbl start
ld.b true
sto wait

ld.arg 0
ld.arg 1
ld.arg 2
ld.arg 3
async async,2

lbl loop
ld.var wait
branch loop
ret").CreateExecutionManager("asdf", "jklö", "hans", "gustav");

            while (exec.Step()) { }

            try
            {
                parser.Create(@"//try catch
hello
lbl start
call a
call b
call c
call d
jmp end

lbl a
call b
call c
call d
reti

lbl b
call c
call d
reti

lbl c
call d
reti

lbl d
ld.arg 0
dup
println
//call end
reti

lbl end
ld.str ""TestMessage""
newobj.i System.Exception,1
throw
ret

lbl catch
println").DynamicInvoke(args: "Test!");
            }
            catch (Exception e)
            {
                Console.Write("[FATAL] ");
                Console.WriteLine(e);
            }

            parser.Create(@"jmp start
lbl handleClient
dup
invoke.vi GetStream,0
newobj.i System.IO.StreamReader,1
invoke.vi ReadToEnd,0
println
invoke.vi Close,0
reti

lbl start
try catch
ld.c 1337
ld.arg 0
ld.c 1
newobj.s
//newobj.i System.Net.Sockets.TcpListener,1
dup
sto listener
invoke.vi Start,0

lbl loop
ld.str ""Waiting...""
println
ld.var listener
invoke.vi AcceptTcpClient,0
call handleClient
jmp loop

lbl catch
println
").DynamicInvoke(args: typeof(TcpListener));

            Console.ReadKey(true);
        }


        class Tmp
        {
            public Tmp(int v)
            {
                Value = v;
            }

            public int Value { get; set; }

            public void Incr()
            {
                Value++;
            }

            public void Decr()
            {
                Value--;
            }

            public bool IsZero()
            {
                return Value == 0;
            }

            public static Tmp operator +(Tmp t1, Tmp t2)
            {
                return new Tmp(t1.Value + t2.Value);
            }

            public static Tmp operator !(Tmp t)
            {
                return new Tmp(t.Value * -1);
            }

            public static bool operator <=(Tmp t1, Tmp t2)
            {
                return t1.Value <= t2.Value;
            }

            public static bool operator >=(Tmp t1, Tmp t2)
            {
                return t1.Value >= t2.Value;
            }
        }
    }
}
