using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.TwitterGnip.SearchApi.Response
{

    public class SearchResponse
    {
        public string next { get; set; }
        public Result[] results { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string objectType { get; set; }
        public Actor actor { get; set; }
        public string verb { get; set; }
        public DateTime postedTime { get; set; }
        public Generator generator { get; set; }
        public Provider provider { get; set; }
        public string link { get; set; }
        public string body { get; set; }
        public Object _object { get; set; }
        public int favoritesCount { get; set; }
        public Twitter_Entities twitter_entities { get; set; }
        public string twitter_filter_level { get; set; }
        public int retweetCount { get; set; }
        public Gnip gnip { get; set; }
    }

    public class Actor
    {
        public string objectType { get; set; }
        public string id { get; set; }
        public string link { get; set; }
        public string displayName { get; set; }
        public DateTime postedTime { get; set; }
        public string image { get; set; }
        public string summary { get; set; }
        public Link[] links { get; set; }
        public int friendsCount { get; set; }
        public int followersCount { get; set; }
        public int listedCount { get; set; }
        public int statusesCount { get; set; }
        public string twitterTimeZone { get; set; }
        public bool verified { get; set; }
        public string utcOffset { get; set; }
        public string preferredUsername { get; set; }
        public string[] languages { get; set; }
        public int favoritesCount { get; set; }
    }

    public class Link
    {
        public object href { get; set; }
        public string rel { get; set; }
    }

    public class Generator
    {
        public string displayName { get; set; }
        public string link { get; set; }
    }

    public class Provider
    {
        public string objectType { get; set; }
        public string displayName { get; set; }
        public string link { get; set; }
    }

    public class Object
    {
        public string objectType { get; set; }
        public string id { get; set; }
        public string summary { get; set; }
        public string link { get; set; }
        public DateTime postedTime { get; set; }
    }

    public class Twitter_Entities
    {
        public object[] hashtags { get; set; }
        public object[] symbols { get; set; }
        public object[] urls { get; set; }
        public object[] user_mentions { get; set; }
    }

    public class Gnip
    {
        public Language language { get; set; }
    }

    public class Language
    {
        public string value { get; set; }
    }

}
