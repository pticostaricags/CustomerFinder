using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [ComplexType]
    public class TwitterNetworkUserStatus
    {
        public long TwitterUserStatusId { get; set; }
        public long TwitterAccountsId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string StatusUrl { get; set; }
        public int FavoriteCount { get; set; }
        public int RetweetCount { get; set; }
        public int StatusCount { get; set; }
        public string TweetId { get; set; }
        public string Username { get; set; }
    }
}
