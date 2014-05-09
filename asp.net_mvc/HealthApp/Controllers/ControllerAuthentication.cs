using System;
using System.Web.Mvc;
namespace healthApp.Controllers
{
    //controllers should extend this class. It contains methods for authentication methods for the users
    public class ControllerAuthentication : Controller
    {
        
        
        public bool hasAdminAccess()
        {
            string[] role = User.Identity.Name.ToString().Split('&');
            return Request.IsAuthenticated && (role[1] == "sysadmin");
        }

        public bool hasUserAccess()
        {
            string[] role = User.Identity.Name.ToString().Split('&');
            //note: name in this case is actually the role of the user not the name
            return Request.IsAuthenticated && (hasAdminAccess() || (role[1] == "user"));
        }
    }
}
