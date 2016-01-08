using System;
using System.Web;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Core.Fakes;
using ViccosLite.Services.Authentication;
using ViccosLite.Services.Helpers;
using ViccosLite.Services.Users;

namespace ViccosLite.Framework.Context
{
    public class WebWorkContext : IWorkContext
    {
        #region Const

        private const string USER_COOKIE_NAME = "Soft.User";

        #endregion

        #region Ctr

        public WebWorkContext(HttpContextBase httpContext, IUserService userService, IUserAgentHelper userAgentHelper,
            IAuthenticationService authenticationService, User cachedUser)
        {
            _httpContext = httpContext;
            _userService = userService;
            _userAgentHelper = userAgentHelper;
            _authenticationService = authenticationService;
            _cachedUser = cachedUser;
        }

        #endregion

        public virtual User CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                User user = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //Revisa cualquier request que haya sido hecho por una tarea de background
                    //En este caso retornamos una construccion para el cliente de la tarea de background
                    user = _userService.GetUserBySystemName(SystemUserNames.BackgroundTask);
                }

                //Revisa si el request es echo por un motor de busqueda
                //En este caso retornamos una construccion para el cliente del motor de busquedas
                //o se puede comentar estas 2 lineas de codigo para desabilitar esta funcionalidadif (customer == null || customer.Deleted || !customer.Active)
                if (user == null || user.Deleted || !user.Active)
                {
                    if (_userAgentHelper.IsSearchEngine())
                        user = _userService.GetUserBySystemName(SystemUserNames.SearchEngine);
                }

                //Usuario registrado
                if (user == null || user.Deleted || !user.Active)
                {
                    user = _authenticationService.GetAuthenticatedUser();
                }

                //validacion
                if (!user.Deleted && user.Active)
                {
                    SetUserCookie(user.UserGuid);
                    _cachedUser = user;
                }

                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedUser = value;
            }
        }
        public virtual bool IsAdmin { get; set; }

        #region Campos

        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly IAuthenticationService _authenticationService;
        private User _cachedUser;

        #endregion

        #region Util

        protected virtual HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[USER_COOKIE_NAME];
        }

        protected virtual void SetUserCookie(Guid userGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(USER_COOKIE_NAME)
                {
                    HttpOnly = true,
                    Value = userGuid.ToString()
                };
                if (userGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    const int COOKIE_EXPIRES = 24*365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(COOKIE_EXPIRES);
                }

                _httpContext.Response.Cookies.Remove(USER_COOKIE_NAME);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion
    }
}