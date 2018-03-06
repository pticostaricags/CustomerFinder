using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerFinderMVCWebSite.Models.ViewModels
{
    public class NetworkStatusInfo
    {
        public long FirstLevelFollowers { get; set; }
        public int MessagesProcessed { get; set; }
        public long SecondLevelFollowers { get; set; }
    }
}