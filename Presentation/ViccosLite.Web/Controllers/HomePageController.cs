using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ViccosLite.Web.Controllers
{
    public class HomePageController : BaseAdminController
    {
        // GET: HomePage
        public ActionResult Index()
        {
            return View();
        }
    }
}