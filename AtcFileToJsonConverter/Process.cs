using AtcFileToJsonConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter
{
    public class Process
    {
        public void Execute(string filePath, string targetPath)
        {
            var atcData = File.ReadAllBytes(filePath);

            AtcParser parser = new AtcParser();

            var parserResponse = parser.Parse(atcData);

            AtcFileResponse response = new AtcFileResponse();
            response.EcgData = parserResponse;

            if (parserResponse.Info != null)
            {
                response.DecodedInfoBlock = new DecodedInfoBlock(parserResponse.Info);
            }

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(targetPath, jsonString);
        }
    }
}