using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveServices
{
    public static class ComputerVision
    {
        class Root
        {
            public string language { get; set; }
            public double textAngle { get; set; }
            public string orientation { get; set; }
            public List<Region> regions { get; set; }
        }

        class Region
        {
            public string boundingBox { get; set; }
            public List<Line> lines { get; set; }
        }

        class Line
        {
            public string boundingBox { get; set; }
            public List<Word> words { get; set; }
        }

        class Word
        {
            public string boundingBox { get; set; }
            public string text { get; set; }
        }

        public static async Task<string> OCRAsync(string file)
        {
            var url = "https://northeurope.api.cognitive.microsoft.com/vision/v2.0/ocr";
            var type = "application/octet-stream";

            var stream = new FileStream(file, FileMode.Open);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);

            var headers = new Dictionary<string, string>();
            headers.Add("Ocp-Apim-Subscription-Key", "e84b8e5786bc441d93818cb2a55d265a");

            var result = await Rest.PostAsync(url, type, headers, data);

            var root = JsonConvert.DeserializeObject<Root>(result);

            var builder = new StringBuilder();

            for (int i = 0; i < root.regions.Count; i++)
            {
                for (int j = 0; j < root.regions[i].lines.Count; j++)
                {
                    for (int k = 0; k < root.regions[i].lines[j].words.Count; k++)
                    {
                        builder.Append(root.regions[i].lines[j].words[k].text);
                        if (k < root.regions[i].lines[j].words.Count - 1)
                            builder.Append(" ");
                    }
                    if (j < root.regions[i].lines.Count - 1)
                        builder.Append("\n");
                }
                if (i < root.regions.Count - 1)
                    builder.Append("\n\n");
            }

            return builder.ToString();
        }
    }
}
