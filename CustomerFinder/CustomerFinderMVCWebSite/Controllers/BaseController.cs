using CustomerFinderMVCWebSite.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CustomerFinderMVCWebSite.Controllers
{
    public class BaseController : Controller
    {
        protected CustomerFinderDA.CustomerFinderContext DbContext
        {
            get
            {
                return new CustomerFinderDA.CustomerFinderContext("CustomerFinderContext");
            }
        }
        protected async Task<ApplicationUser> GetLoggedInUserInfo()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            return user;
        }
    }
}