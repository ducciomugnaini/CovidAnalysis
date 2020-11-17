using Newtonsoft.Json.Linq;
using RealtimeCompiler.Interfaces;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DynamicRun.Builder
{
    internal class Runner
    {
        public JObject Execute(byte[] compiledAssembly, JObject json)
        {
            var tupleResult = LoadAndExecute(compiledAssembly, json);

            Unload(tupleResult.Item1);
            
            return tupleResult.Item2;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Tuple<WeakReference, JObject> LoadAndExecute(byte[] compiledAssembly, JObject json)
        {
            using (var asm = new MemoryStream(compiledAssembly))
            {
                var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();
                var assembly = assemblyLoadContext.LoadFromStream(asm);

                IRunnable p = (IRunnable) assembly.CreateInstance("DynamicProgram.DynamicManipulation");
                JObject jsonResult = null;

                if (!(p == null))
                    jsonResult = p.Elaborate(new JObject());                
                else
                    Console.WriteLine("Unable to instantiate the desired DynamicClass.");
                
                assemblyLoadContext.Unload();

                return new Tuple<WeakReference, JObject>(new WeakReference(assemblyLoadContext), jsonResult);
            }
        }

        private static void Unload(WeakReference weakReference)
        {
            for (var i = 0; i < 8 && weakReference.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Console.WriteLine(weakReference.IsAlive ? "Unloading failed!" : "Unloading success!");
        }
    }
}