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
            string json =
                @"{'data': [
                    {
                        'col_1': 123,
                        'col_2': 234,
                        'col_3': 345,
                        'col_4': 456,
                        'col_5': 567,
                        'col_6': 'asd'
                    },
                    {
                        'col_1': 111,
                        'col_2': 222,
                        'col_3': 333,
                        'col_4': 444,
                        'col_5': 555,
                        'col_6': 'qwe'
                    },
                    {
                        'col_1': 666,
                        'col_2': 777,
                        'col_3': 888,
                        'col_4': 999,
                        'col_5': 111,
                        'col_6': 'zxc'
                    }
                   ]}";

            // JSON -> Object
            // https://www.newtonsoft.com/json/help/html/DeserializeObject.htm
            // https://www.newtonsoft.com/json/help/html/DeserializeCollection.htm

            JObject jsonInput = JObject.Parse(json);

            var jsonResult = RealtimeCompiler.RealtimeCompiler.Run(jsonInput);

            Assert.Pass();
        }

        [Test]
        public void TestTemplate()
        {

            var jsonResult = RealtimeCompiler.RealtimeCompiler.RunWithTemplate(null, null);
            Assert.Pass();
        }
    }
}