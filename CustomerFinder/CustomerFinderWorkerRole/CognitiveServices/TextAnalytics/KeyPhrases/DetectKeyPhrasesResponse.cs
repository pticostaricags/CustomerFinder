using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.CognitiveServices.TextAnalytics.KeyPhrases.Response
{

    public class DetectKeyPhrasesResponse
    {
        public Document[] documents { get; set; }
        public Error[] errors { get; set; }
    }

    public class Document
    {
        public string[] keyPhrases { get; set; }
        public string id { get; set; }
    }

    public class Error
    {
        public string id { get; set; }
        public string message { get; set; }
    }

}
