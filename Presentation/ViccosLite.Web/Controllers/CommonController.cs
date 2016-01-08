using System.Web.Mvc;

namespace ViccosLite.Web.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}