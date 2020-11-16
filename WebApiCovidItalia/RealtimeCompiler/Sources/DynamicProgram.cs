using System;
using Newtonsoft.Json.Linq;
using RealtimeCompiler.Interfaces;

/*namespace Hello
{
    class DynamicProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello damians from {args[0]}!");
        }
    }
}*/

namespace DynamicProgram
{
    public class DynamicManipulation : IRunnable
    {
        public JObject Elaborate(JObject data)
        {
            string json = @"{
            CPU: 'Intel',
            Drives: [
                'DVD read/writer',
                '500 gigabyte hard drive'
            ]
            }";

            JObject o = JObject.Parse(json);
            return o;
        }
    }
}