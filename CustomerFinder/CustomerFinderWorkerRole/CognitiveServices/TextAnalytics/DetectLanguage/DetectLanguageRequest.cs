using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.CognitiveServices.TextAnalytics.DetectLanguage.Request
{

    public class DetectLanguageRequest
    {
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public string text { get; set; }
    }

}
