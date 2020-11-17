using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealtimeCompiler.Interfaces;

namespace DynamicProgram
{
    public class DynamicManipulation : IRunnable
    {
        private class InputDataStructure
        {
            public double col_1{ get; set; }
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

        public JObject Elaborate(JObject data)
        {
            // ---------------------------------------------------------------------------------------------------------- ||
            // NB: non utilizzare i commenti multilinea => il codice incluso viene cmq compilato e può generare errori!!! ||
            // ---------------------------------------------------------------------------------------------------------- ||

            // json => list
            // https://www.newtonsoft.com/json/help/html/DeserializeObject.htm
            // JObject => list
            // https://www.newtonsoft.com/json/help/html/ToObjectComplex.htm

            JArray dataJArray = (JArray)data["data"];
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
            outputJObject.Add("elaboratedData", jsonArray);

            return outputJObject;
        }
    }
}