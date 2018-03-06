using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.CognitiveServices.TextAnalytics.Sentiment.Response
{

    public class SentimentResponse
    {
        public Document[] documents { get; set; }
        public Error[] errors { get; set; }
    }

    public class Document
    {
        public float score { get; set; }
        public string id { get; set; }
    }

    public class Error
    {
        public string id { get; set; }
        public string message { get; set; }
    }

}
