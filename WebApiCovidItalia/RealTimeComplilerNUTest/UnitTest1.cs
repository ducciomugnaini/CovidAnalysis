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

            _ = RealtimeCompiler.RealtimeCompiler.Run("Implementation");

            Assert.Pass();
        }
    }
}