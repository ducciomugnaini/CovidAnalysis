using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reactive.Linq;
using DynamicRun.Builder;

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

            var compiler = new Compiler();
            var runner = new Runner();

            var compiledAssembly = compiler.Compile(dynamicElaboatorClassPath);
            var jsonElaborated = runner.Execute(compiledAssembly, jsonToElaborate);

            return jsonElaborated;
        }

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
                    runner.Execute(compiler.Compile(filepath), new JObject())
                   );

                watcher.Start();

                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();
            }
        }
    }
}
