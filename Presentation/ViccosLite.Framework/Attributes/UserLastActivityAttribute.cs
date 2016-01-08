using System;
using System.Web.Mvc;
using ViccosLite.Core;
using ViccosLite.Core.Data;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Services.Users;

namespace ViccosLite.Framework.Attributes
{
    public class UserLastActivityAttribute : ActionFilterAttribute
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

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var user = workContext.CurrentUser;

            //Actualiza la fecha de la ultima actividad minuto a minuto
            if (user.LastActivityDateUtc.AddMinutes(1.0) >= DateTime.UtcNow)
                return;

            var userService = EngineContext.Current.Resolve<IUserService>();
            user.LastActivityDateUtc = DateTime.UtcNow;
            userService.UpdateUser(user);
        }
    }
}