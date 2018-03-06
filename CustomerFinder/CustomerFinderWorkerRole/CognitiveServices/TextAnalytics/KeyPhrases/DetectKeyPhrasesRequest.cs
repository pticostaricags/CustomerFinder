using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.CognitiveServices.TextAnalytics.KeyPhrases.Request
{

    public class DetectKeyPhrasesRequest
    {
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string language { get; set; }
        public string id { get; set; }
        public string text { get; set; }
    }

}
