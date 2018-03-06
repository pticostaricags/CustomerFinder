using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CustomerFinderMVCWebSite.Controllers
{
    [Authorize]
    public class NetworkStatusController : BaseController
    {
        // GET: NetworkStatus
        public async Task<ActionResult> Index()
        {
            Models.ViewModels.NetworkStatusInfo result = new Models.ViewModels.NetworkStatusInfo();
            try
            {
                var loggedInUserInfo = await base.GetLoggedInUserInfo();
                using (CustomerFinderDA.CustomerFinderContext ctx = base.DbContext)
                {
                    //string twitterUsername = loggedInUserInfo.TwitterUsername;
                    string twitterUsername = "starbucks_cr";
                    var firstLevelFollowersCount = ctx.fnGetTwitterFirstLevelFollowersIds(twitterUsername).ToList();
                    var secondLevelFollowersCount = ctx.fnGetTwitterSecondLevelFollowersIds(twitterUsername).ToList();
                    var messagesProcessedCount = ctx.fnGetNetworkTwitterUserStatus(twitterUsername).Count();
                    result.FirstLevelFollowers = firstLevelFollowersCount.Count;
                    result.SecondLevelFollowers = secondLevelFollowersCount.Count;
                    result.MessagesProcessed = messagesProcessedCount;
                }
            }
            catch (Exception ex)
            {

            }
            return View(result);
        }
    }
}