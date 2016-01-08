using System.Web.Routing;

namespace ViccosLite.Framework.Mvc.Routes
{
    public interface IRoutePublisher
    {
        void RegisterRoutes(RouteCollection routes); 
    }
}