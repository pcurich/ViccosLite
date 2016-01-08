using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using ViccosLite.Core.Infrastructure;

namespace ViccosLite.Framework.Mvc.Routes
{
    public class RoutePublisher : IRoutePublisher
    {
        protected readonly ITypeFinder TypeFinder;

        /// <summary>
        ///     Inicializa una nueva clase de <see cref="RoutePublisher" />
        /// </summary>
        /// <param name="typeFinder">El típo encontrado</param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            TypeFinder = typeFinder;
        }

        /// <summary>
        ///     Routes registrados
        /// </summary>
        /// <param name="routes">Routes</param>
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            var routeProviderTypes = TypeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                //Ignore not installed plugins
                var plugin = FindPlugin(providerType);
                //if (plugin != null && !plugin.Installed)
                  //  continue;

                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }
            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterRoutes(routes));
        }

        /// <summary>
        ///     Encuentra los descriptores de los plugins por algun tipo que este localizable en un ensamblado
        /// </summary>
        /// <param name="providerType">Provider type</param>
        /// <returns>Descriptor del plugin</returns>
        protected virtual object FindPlugin(Type providerType)
        {
            return null;
        }

        //protected virtual PluginDescriptor FindPlugin(Type providerType)
        //{
            //if (providerType == null)
            //    throw new ArgumentNullException("providerType");

            //foreach (var plugin in PluginManager.ReferencedPlugins)
            //{
            //    if (plugin.ReferencedAssembly == null)
            //        continue;

            //    if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
            //        return plugin;
            //}

        //    return null;
        //}
    }
}