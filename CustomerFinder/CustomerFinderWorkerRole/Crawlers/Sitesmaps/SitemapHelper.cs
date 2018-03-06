using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.Crawlers.Sitesmaps
{
    public class SitemapHelper
    {
        public void ProcessSitemapForUrl(string weburl, bool isShortened)
        {
            try
            {
                string sitemapUrl = weburl;
                if (isShortened)
                {
                    System.Net.WebRequest expandRequest = System.Net.WebRequest.Create(weburl);
                    var expandResponse = expandRequest.GetResponse();
                    string expandedUrl = expandResponse.ResponseUri.ToString();
                    sitemapUrl = expandedUrl;
                }

                var defaultSitemapPossibleUrl = sitemapUrl.TrimStart('/');
                defaultSitemapPossibleUrl = string.Format("{0}/sitemap.xml", defaultSitemapPossibleUrl);
                System.Uri uri = new Uri(defaultSitemapPossibleUrl);
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                    webClient.DownloadStringAsync(uri);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void WebClient_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Crawlers.Sitesmaps.urlset));
                    using (System.IO.StringReader reader = new System.IO.StringReader(e.Result))
                    {
                        Crawlers.Sitesmaps.urlset sitemap = serializer.Deserialize(reader) as Crawlers.Sitesmaps.urlset;
                        if (sitemap != null)
                        {
                            foreach (var singleItem in sitemap.url)
                            {
                                int locPos = -1;
                                for (int iPos = 0; iPos < singleItem.ItemsElementName.Length; iPos++)
                                {
                                    if (singleItem.ItemsElementName[iPos] == ItemsChoiceType.loc)
                                    {
                                        locPos = iPos;
                                        break;
                                    }
                                }
                                if (locPos > 0)
                                {
                                    var locUrl = Convert.ToString(singleItem.Items[locPos]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {

            }
        }
    }
}
