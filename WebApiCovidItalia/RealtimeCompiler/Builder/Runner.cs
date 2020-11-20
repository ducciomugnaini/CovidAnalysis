using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealtimeCompiler.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace DynamicRun.Builder
{
    internal class Runner
    {
        private class InputDataStructure
        {
            public double col_1 { get; set; }
            public double col_2 { get; set; }
            public double col_3 { get; set; }
            public double col_4 { get; set; }
            public double col_5 { get; set; }
            public string col_6 { get; set; }
        }

        private class OutputDataStructure
        {
            public double out_1 { get; set; }
            public string out_2 { get; set; }
        }

        public JObject Execute(string classNameWithNameSpace, byte[] compiledAssembly, JObject json)
        {
            var tupleResult = LoadAndExecute(classNameWithNameSpace, compiledAssembly, json);

            Unload(tupleResult.Item1);
            
            return tupleResult.Item2;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Tuple<WeakReference, JObject> LoadAndExecute(string classNameWithNameSpace, byte[] compiledAssembly, JObject json)
        {
            using (var asm = new MemoryStream(compiledAssembly))
            {
                var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();
                var assembly = assemblyLoadContext.LoadFromStream(asm);

                IRunnable p = (IRunnable) assembly.CreateInstance(classNameWithNameSpace);
                JObject jsonResult = null;

                // ---------------------------------------------------------------------------------------

                /*JArray dataJArray = (JArray)json["data"];
                IList<InputDataStructure> inputList = dataJArray.ToObject<IList<InputDataStructure>>();

                List<OutputDataStructure> outputList = new List<OutputDataStructure>();

                foreach (var currInput in inputList)
                {
                    outputList.Add(new OutputDataStructure
                    {
                        out_1 = currInput.col_1 + currInput.col_2,
                        out_2 = currInput.col_6
                    });
                }

                // List => Json
                // https://www.newtonsoft.com/json/help/html/SerializingCollections.htm

                string outputJson = JsonConvert.SerializeObject(outputList, Formatting.Indented);
                JArray jsonArray = JArray.Parse(outputJson);

                var outputJObject = new JObject();
                outputJObject.Add("elaboratedData", jsonArray);*/

                // ---------------------------------------------------------------------------------------

                if (!(p == null))
                    jsonResult = p.Elaborate(json);                
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