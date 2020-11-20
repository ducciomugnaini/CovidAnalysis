using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reactive.Linq;
using DynamicRun.Builder;
using Scriban;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Dynamic;
using Scriban.Runtime;

namespace RealtimeCompiler
{
    public static class RealtimeCompiler
    {

        public static JObject Run(JObject jsonToElaborate, string dynamicElaboatorClassPath = @".\Sources\DynamicProgram.cs")
        {
            var sourcesPath = Path.Combine(Environment.CurrentDirectory, "Sources");

            Console.WriteLine($"Running from: {Environment.CurrentDirectory}");
            Console.WriteLine($"Sources from: {sourcesPath}");
            Console.WriteLine("Modify the sources to compile and run it!");

            // -- compile and run class

            var compiler = new Compiler();
            var runner = new Runner();

            var compiledAssembly = compiler.CompileFile(dynamicElaboatorClassPath);
            var jsonElaborated = runner.Execute(compiledAssembly, jsonToElaborate);

            return jsonElaborated;
        }

        public static JObject RunWithTemplate(JObject jsonToElaborate, JObject DataStructure, 
            string dynamicTemplateElaboatorClassPath = @".\Sources\DynamicScribanTemplate.txt")
        {
            // -- compile template
            var sourcesPath = Path.Combine(Environment.CurrentDirectory, "Sources");
            string templateFromText = File.ReadAllText(dynamicTemplateElaboatorClassPath);


            var structureJson =
                @"{
                    'inputFields' : [
                        {
                            fieldName : 'col_1',
                            fieldType : 'double'
                        },
                        {
                            fieldName : 'col_2',
                            fieldType : 'double'
                        },
                    ],
                    'outputFields' : [
                        {
                            fieldName : 'out_1',
                            fieldType : 'double',
                            fieldElab : '_.col_1 + _.col_2'
                        }

                    ]
                }";

            var renderedTemplate = ScribanRenderer.RenderJson(structureJson, templateFromText);


            // -- compile and run class

            var compiler = new Compiler();
            var runner = new Runner();

            var compiledAssembly = compiler.CompileSource(renderedTemplate);
            var jsonElaborated = runner.Execute(compiledAssembly, jsonToElaborate);

            return jsonElaborated;
        }

        // dismissing method
        public static void Observe(string classImplementation)
        {
            var sourcesPath = Path.Combine(Environment.CurrentDirectory, "Sources");

            Console.WriteLine($"Running from: {Environment.CurrentDirectory}");
            Console.WriteLine($"Sources from: {sourcesPath}");
            Console.WriteLine("Modify the sources to compile and run it!");

            var compiler = new Compiler();
            var runner = new Runner();

            using (var watcher = new ObservableFileSystemWatcher(c => { c.Path = @".\Sources"; }))
            {
                var changes = watcher.Changed.Throttle(TimeSpan.FromSeconds(.5)).Where(c => c.FullPath.EndsWith(@"DynamicProgram.cs")).Select(c => c.FullPath);

                changes.Subscribe(filepath =>
                    runner.Execute(compiler.CompileFile(filepath), new JObject())
                   );

                watcher.Start();

                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();
            }
        }
    }

    public static class ScribanRenderer
    {
        public static string RenderJson(string json, string content)
        {

            var expando = JsonConvert.DeserializeObject<ExpandoObject>(json);
            var sObject = BuildScriptObject(expando);
            var templateCtx = new Scriban.TemplateContext();
            templateCtx.PushGlobal(sObject);
            var template = Scriban.Template.Parse(content);
            var result = template.Render(templateCtx);

            return result;
        }

        private static ScriptObject BuildScriptObject(ExpandoObject expando)
        {
            var dict = (IDictionary<string, object>)expando;
            var scriptObject = new ScriptObject();

            foreach (var kv in dict)
            {
                var renamedKey = StandardMemberRenamer.Rename(kv.Key);

                if (kv.Value is ExpandoObject expandoValue)
                {
                    scriptObject.Add(renamedKey, BuildScriptObject(expandoValue));
                }
                else
                {
                    scriptObject.Add(renamedKey, kv.Value);
                }
            }

            return scriptObject;
        }
    }
}
