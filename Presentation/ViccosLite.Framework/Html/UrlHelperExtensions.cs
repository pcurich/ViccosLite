using System.Web.Mvc;

namespace ViccosLite.Framework.Html
{
    public static class UrlHelperExtensions
    {
        public static string LogOn(this UrlHelper urlHelper, string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl)
                ? urlHelper.Action("Login", "User", new { ReturnUrl = returnUrl })
                : urlHelper.Action("Login", "User");
        }

        public static string LogOff(this UrlHelper urlHelper, string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl)
                ? urlHelper.Action("Logout", "User", new { ReturnUrl = returnUrl })
                : urlHelper.Action("Logout", "User");
        }
    }
}