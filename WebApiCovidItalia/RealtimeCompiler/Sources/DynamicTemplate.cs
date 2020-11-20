using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealtimeCompiler.Interfaces;

namespace DynamicProgram
{
    public class DynamicTemplate : IRunnable
    {
        // => in config -> generano la classe privata InputDataStructure
        /*
         * InputDataStructure : [
         *  {
         *      fieldName : 'col_1',
         *      fieldType : 'double'
         *  },
         *  {
         *      fieldName : 'col_2',
         *      fieldType : 'double'
         *  },
         *  ...
         * ]
         */
        private class InputDataStructure
        {
            public double col_1 { get; set; }
            public double col_2 { get; set; }
            public double col_3 { get; set; }
            public double col_4 { get; set; }
            public double col_5 { get; set; }
            public string col_6 { get; set; }
        }

        // => in config -> generano la classe privata OutputDataStructure
        /*
         * OutputDataStructure : [
         *  {
         *      fieldName : 'out_1',
         *      fieldType : 'double',
         *      fieldElab : '_.col_1 + _.col_2'
         *  },
         *  {
         *      fieldName : 'out_2',
         *      fieldType : 'string'
         *      fieldElab : '_.col_6'
         *  },
         *  {
         *      fieldName : 'out_3',
         *      fieldType : 'double'
         *      fieldElab : '_.col_4 * _.col_1'
         *  }
         * ]
         */
        private class OutputDataStructure
        {
            public double out_1 { get; set; }
            public string out_2 { get; set; }
            public double out_3 { get; set; }

            public static Func<InputDataStructure, double> compute_out_1 = (_) =>
            {
                return _.col_1 + _.col_2;
            };
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

            foreach (var _ in inputList)
            {
                var output = new OutputDataStructure();
                output.out_1 = OutputDataStructure.compute_out_1(_);   // <= !! from config
                output.out_2 = _.col_6;                                // <= !! from config
                output.out_3 = _.col_4 * _.col_1;                       // <= !! from config
                outputList.Add(output);

                /*outputList.Add(new OutputDataStructure
                {
                    out_1 = OutputDataStructure.compute_out_1(_),   // <= !! from config
                    out_2 = _.col_6,                                // <= !! from config
                    out_3 = _.col_4 * _.col_1                       // <= !! from config
                });*/
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