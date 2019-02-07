using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveServices
{
    public static class TextAnalytics
    {
        class Root
        {
            public Document[] documents { get; set; }
        }

        class Document
        {
            public string language { get; set; }
            public string id { get; set; }
            public string text { get; set; }
            public Language[] detectedLanguages { get; set; }
            public string[] keyPhrases { get; set; }
        }

        class Language
        {
            public string name { get; set; }
            public string iso6391Name { get; set; }
            public double score { get; set; }
        }

        public static async Task<string[]> DetectLanguageAsync(string[] text)
        {
            var url = "https://northeurope.api.cognitive.microsoft.com/text/analytics/v2.0/languages";
            var type = "application/json";

            var root = new Root();
            root.documents = new Document[text.Length];
            for (int i = 0; i < text.Length; i++)
                root.documents[i] = new Document() { id = (i + 1).ToString(), text = text[i] };

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(root, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

            var headers = new Dictionary<string, string>();
            headers.Add("Ocp-Apim-Subscription-Key", "f42769a504004f089d278301e7068ae0");

            var result = await Rest.PostAsync(url, type, headers, data);

            root = JsonConvert.DeserializeObject<Root>(result);

            var languages = new string[text.Length];
            foreach (var document in root.documents)
                languages[int.Parse(document.id) - 1] = document.detectedLanguages[0].iso6391Name;

            return languages;
        }

        public static async Task<string[][]> KeyPhrasesAsync(string[] text)
        {
            var languages = await DetectLanguageAsync(text);

            var url = "https://northeurope.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";
            var type = "application/json";

            var root = new Root();
            root.documents = new Document[text.Length];
            for (int i = 0; i < text.Length; i++)
                root.documents[i] = new Document() { language = languages[i], id = (i + 1).ToString(), text = text[i] };

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(root, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

            var headers = new Dictionary<string, string>();
            headers.Add("Ocp-Apim-Subscription-Key", "f42769a504004f089d278301e7068ae0");

            var result = await Rest.PostAsync(url, type, headers, data);

            root = JsonConvert.DeserializeObject<Root>(result);

            var keyPhrases = new string[text.Length][];
            foreach (var document in root.documents)
                if (document.keyPhrases != null)
                    keyPhrases[int.Parse(document.id) - 1] = document.keyPhrases;

            return keyPhrases;
        }
    }
}
