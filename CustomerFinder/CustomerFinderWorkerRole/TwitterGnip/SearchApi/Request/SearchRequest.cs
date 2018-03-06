using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.TwitterGnip.SearchApi.Request
{

    public class SearchRequest
    {
        public string publisher { get; set; }
        public string query { get; set; }
        public string maxResults { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string next { get; set; }
    }

}
