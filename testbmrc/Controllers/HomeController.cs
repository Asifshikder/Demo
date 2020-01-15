using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    [Authorize]
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize(Roles = "1,2,3,4")]
        public ActionResult DashBoard()
        {
            return View();
        }
    }
}