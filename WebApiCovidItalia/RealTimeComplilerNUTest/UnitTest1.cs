using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RealTimeComplilerNUTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            string json = @"{
                myCPU: 'AMD Radeon',
                myDrives: ['A', 'B']
                }";

            JObject jsonInput = JObject.Parse(json);

            var jsonResult = RealtimeCompiler.RealtimeCompiler.Run(jsonInput);

            Assert.Pass();
        }
    }
}