using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Framework.Controllers;

namespace ViccosLite.Web.Controllers
{
    public abstract class BasePublicController : BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            //Llama al objetivo del controlador y le pasa el routeData
            IController errorController = EngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(HttpContext, routeData));

            return new EmptyResult();
        }
    }
}