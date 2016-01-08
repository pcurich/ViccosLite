using System;
using System.Web.Mvc;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Settings.Security;
using ViccosLite.Core.Infrastructure;

namespace ViccosLite.Framework.Attributes.Admin
{
    public class AdminValidateIpAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            var request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            //No se aplica el filtro a los metodos hijos
            if (filterContext.IsChildAction)
                return;

            var ok = false;
            var ipAddresses = EngineContext.Current.Resolve<SecuritySettings>().AdminAreaAllowedIpAddresses;
            if (ipAddresses != null && ipAddresses.Count > 0)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                foreach (var ip in ipAddresses)
                    if (ip.Equals(webHelper.GetCurrentIpAddress(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        ok = true;
                        break;
                    }
            }
            else
            {
                //no restrictions
                ok = true;
            }

            if (!ok)
            {
                //Se asegura que no sea 'Acceso negado' a la paguina
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                var thisPageUrl = webHelper.GetThisPageUrl(false);
                if (
                    !thisPageUrl.StartsWith(
                        String.Format("{0}admin/security/accessdenied", webHelper.GetStoreLocation()),
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    //redireccionar a la pagina 'Access denied'
                    filterContext.Result =
                        new RedirectResult(webHelper.GetStoreLocation() + "admin/security/accessdenied");
                    //filterContext.Result = RedirectToAction("AccessDenied", "Security");
                }
            }
        }
    }
}