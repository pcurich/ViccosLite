using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;


namespace ViccosLite.Core.Infrastructure.DependencyManagement
{
    public class ContainerManager
    {
        public ContainerManager(IContainer container)
        {
            Container = container;
        }

        public IContainer Container { get; private set; }

        public T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
                //objetivo no especificado
                scope = Scope();

            return string.IsNullOrEmpty(key)
                ? scope.Resolve<T>()
                : scope.ResolveKeyed<T>(key);
        }

        public object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
                //objetivo  no especificado
                scope = Scope();

            return scope.Resolve(type);
        }

        public T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
                //objetivo  no especificado
                scope = Scope();

            return string.IsNullOrEmpty(key)
                ? scope.Resolve<IEnumerable<T>>().ToArray()
                : scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        public T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof (T), scope) as T;
        }

        public object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
                //objetivo  no especificado
                scope = Scope();

            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null)
                            throw new KsException("Dependencia desconocida");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (KsException)
                {
                }
            }
            throw new KsException("No fue encontrado ningun contructor.Se tiene todas sus dependencias satisfechas.");
        }

        public bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
                //objetivo  no especificado
                scope = Scope();

            return scope.TryResolve(serviceType, out instance);
        }

        public bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
                //objetivo  no especificado
                scope = Scope();

            return scope.IsRegistered(serviceType);
        }

        public object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
                //objetivo  no especificado
                scope = Scope();

            return scope.ResolveOptional(serviceType);
        }

        public ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                //Cuando el lifetime es regresado, se deberia estar seguro que se dispose una vez usado (por ejemplo en una tarea programada)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                //Cuando se tiene una excepcion aqui si RequestLifetimeScope esta disposado
                //xe un request esta  despues de Application_EndRequest handler
                //Esto usualmente munca pasa

                //Cuando el lifetime es regresado, se deberia estar seguro que se dispose una vez usado (xe en una tarea programada)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}