using Newtonsoft.Json.Linq;
using RealtimeCompiler.Interfaces;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DynamicRun.Builder
{
    internal class Runner
    {
        public void Execute(byte[] compiledAssembly, string[] args)
        {
            var assemblyLoadContextWeakRef = LoadAndExecute(compiledAssembly, args);

            for (var i = 0; i < 8 && assemblyLoadContextWeakRef.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Console.WriteLine(assemblyLoadContextWeakRef.IsAlive ? "Unloading failed!" : "Unloading success!");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference LoadAndExecute(byte[] compiledAssembly, string[] args)
        {
            using (var asm = new MemoryStream(compiledAssembly))
            {
                var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();

                var assembly = assemblyLoadContext.LoadFromStream(asm);

                // ---

                IRunnable p = (IRunnable) assembly.CreateInstance("DynamicProgram.DynamicManipulation");
                if (!(p == null))
                {
                    var res = p.Elaborate(new JObject());
                }
                else
                {
                    Console.WriteLine("Unable to instantiate a Person object.");
                }

                // ---

                var entry = assembly.EntryPoint;

                _ = entry != null && entry.GetParameters().Length > 0
                    ? entry.Invoke(null, new object[] {args})
                    : entry.Invoke(null, null);

                assemblyLoadContext.Unload();

                return new WeakReference(assemblyLoadContext);
            }
        }
    }
}