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
            string classNameWithNameSpace = "DynamicProgram.DynamicManipulation";

            string jsonToElaborateStr =
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

            JObject jsonInput = JObject.Parse(jsonToElaborateStr);

            var jsonResult = RealtimeCompiler.RealtimeCompiler.Run(jsonInput, classNameWithNameSpace);

            Assert.Pass();
        }

        [Test]
        public void TestTemplate()
        {

            string classNameWithNameSpace = "DynamicProgram.DynamicScribanTemplate";

            string jsonToElaborateStr =
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

            var jsonDataStructureStr =
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

            JObject jsonInput = JObject.Parse(jsonToElaborateStr);
            JObject jsonDataStructure = JObject.Parse(jsonDataStructureStr);

            var jsonResult = RealtimeCompiler.RealtimeCompiler.RunWithTemplate(jsonInput, jsonDataStructure, classNameWithNameSpace);

            Assert.Pass();
        }
    }
}