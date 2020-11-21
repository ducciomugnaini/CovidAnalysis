using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace DataAnalysisWeb.Classes
{
    public class Utilities
    {
        public static void ExtractFields(FileStream stream, string delimiter)
        {
            stream.Position = 0;
            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                while (parser.LineNumber == 1)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        //TODO: Process field
                    }
                }
            }
        }
    }
}