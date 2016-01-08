using System;
using System.Web.Mvc;
using ViccosLite.Core;
using ViccosLite.Core.Data;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Services.Users;

namespace ViccosLite.Framework.Attributes
{
    public class StoreIpAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //No se aplica el filtro a los metodos hijos
            if (filterContext.IsChildAction)
                return;

            //Solo se aplica a los request
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var currentIpAddress = webHelper.GetCurrentIpAddress();

            if (String.IsNullOrEmpty(currentIpAddress))
                return;

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var user = workContext.CurrentUser;

            //Actualiza la direccion de IP del cliente que accedio
            if (currentIpAddress.Equals(user.LastIpAddress, StringComparison.InvariantCultureIgnoreCase))
                return;

            var userService = EngineContext.Current.Resolve<IUserService>();
            user.LastIpAddress = currentIpAddress;
            userService.UpdateUser(user);
        }
    }
}