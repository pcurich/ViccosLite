using System.Web.Routing;

namespace ViccosLite.Framework.Mvc.Routes
{
    public interface IRouteProvider
    {
        int Priority { get; }
        void RegisterRoutes(RouteCollection routes);
    }
}