using System.Web.Mvc;

namespace ViccosLite.Web.Controllers
{
    //No se hereda desde el BasePublicController.
    //De otro modo, muchos filtros de accion serian llamados 
    //Ellos pueden crear una cuenta de user , etc

    public class KeepAliveController : Controller
    {
        public ActionResult Index()
        {
            return Content("Estoy vivo!");
        }
    }
}