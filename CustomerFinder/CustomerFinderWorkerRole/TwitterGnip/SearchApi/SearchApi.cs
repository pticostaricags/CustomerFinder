using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.TwitterGnip.SearchApi
{
    public class SearchApi
    {
        private const string SearchApiEndPoint = "https://search.gnip.com/accounts/pticostarica/search/prod.json";

        private const string SearchApiPassword = "";
        private const string SearchApiUsername = "";

        private string AuthInfo { get; } = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", SearchApiUsername, SearchApiPassword)));

        public async Task<TwitterGnip.SearchApi.Response.SearchResponse> GetTweetFromUser(string twitterUsername)
        {
            TwitterGnip.SearchApi.Response.SearchResponse result = null;
            System.Net.ServicePointManager.Expect100Continue = false;
            using (System.Net.Http.WebRequestHandler httpHandler = new System.Net.Http.WebRequestHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip,
            }
            )
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient(httpHandler))
                {
                    TwitterGnip.SearchApi.Request.SearchRequest request = new Request.SearchRequest()
                    {
                        query = "from: [REPLACE]"
                    };
                    string jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    System.Net.Http.StringContent strContent = new System.Net.Http.StringContent(jsonRequest);
                    strContent.Headers.Add("Authorization", string.Format("Basic: {0}", AuthInfo));
                    strContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(SearchApiEndPoint, strContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<TwitterGnip.SearchApi.Response.SearchResponse>(jsonResponse);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            return result;
        }
    }
}
