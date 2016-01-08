using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ViccosLite.Core.Infrastructure
{
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region Ctr

        public AppDomainTypeFinder()
        {
            AssemblyRestrictToLoadingPattern = ".*";
            AssemblySkipLoadingPattern =
                "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";
            AssemblyNames = new List<string>();
            LoadAppDomainAssemblies = true;
        }

        #endregion

        #region Propiedades

        private const bool IGNORE_REFLECTION_ERRORS = true;

        /// <summary>
        ///     El dominio de la aplicacion se bloquea para los tipos de entrada
        /// </summary>
        public virtual AppDomain App
        {
            get { return AppDomain.CurrentDomain; }
        }

        /// <summary>
        ///     Establece o se obtiene cualquier ensamblado en el
        ///     dominio de la aplicacion cuando se cargan los tipos soft
        ///     Cargando los patrones son sumistrados cuando son cargados
        /// </summary>
        public bool LoadAppDomainAssemblies { get; set; }

        /// <summary>
        ///     Obtiene y establece los ensamblados cargados al iniciar
        ///     la aplicacion
        /// </summary>
        public IList<string> AssemblyNames { get; set; }

        /// <summary>
        ///     Obtiene los patrones para las dlls que no necesitamos
        ///     saber para investigarlos
        /// </summary>
        public string AssemblySkipLoadingPattern { get; set; }

        /// <summary>
        ///     Aquellos ensanblados que no necesitamos
        /// </summary>
        public string AssemblyRestrictToLoadingPattern { get; set; }

        #endregion

        #region Metodos

        /// <summary>
        ///     Encuentra clases de tipo T
        /// </summary>
        /// <typeparam name="T">Tipo de clase a buscar</typeparam>
        /// <param name="onlyConcreteClasses">Si es <c>true</c> [solo clases concretas].</param>
        /// <returns></returns>
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        /// <summary>
        ///     Encuentra clases de tipo Type
        /// </summary>
        /// <param name="assignTypeFrom">Tipo de clase a buscar.</param>
        /// <param name="onlyConcreteClasses">Si es <c>true</c> [solo clases concretas].</param>
        /// <returns></returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        /// <summary>
        ///     Encuentra clases Dde tipo T
        /// </summary>
        /// <typeparam name="T">Tipo de clase a buscar</typeparam>
        /// <param name="assemblies">Lista de ensambaldos de donde debe buscar</param>
        /// <param name="onlyConcreteClasses">Si es <c>true</c> [solo clases concretas].</param>
        /// <returns></returns>
        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        /// <summary>
        ///     Encuentra clases de tipo Type
        /// </summary>
        /// <param name="assignTypeFrom">Tipo de clase a buscar.</param>
        /// <param name="assemblies">Lista de ensambaldos de donde debe buscar</param>
        /// <param name="onlyConcreteClasses">Si es <c>true</c> [solo clases concretas].</param>
        /// <returns></returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch
                    {
                        //Entity Framework 6 no permite obtener tipos'(throws una exception)
                        if (!IGNORE_REFLECTION_ERRORS)
                        {
                            throw;
                        }
                    }
                    if (types == null)
                        continue;

                    foreach (var t in types)
                    {
                        if (assignTypeFrom.IsAssignableFrom(t) ||
                            (assignTypeFrom.IsGenericTypeDefinition &&
                             DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                        {
                            if (!t.IsInterface)
                            {
                                if (onlyConcreteClasses)
                                {
                                    if (t.IsClass && !t.IsAbstract)
                                    {
                                        result.Add(t);
                                    }
                                }
                                else
                                {
                                    result.Add(t);
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }

        /// <summary>
        ///     Lista de ensamblados relacionados a la aplicacion actual, usualmente librerias dll
        /// </summary>
        public virtual IList<Assembly> GetAssemblies()
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (LoadAppDomainAssemblies)
                AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        #endregion

        #region Util

        /// <summary>
        ///     Itera todos los ensamblados de la aplicacin y si el nombre coincide
        ///     con el patron de configuracion se agrega a la lista
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Matches(assembly.FullName))
                {
                    if (!addedAssemblyNames.Contains(assembly.FullName))
                    {
                        assemblies.Add(assembly);
                        addedAssemblyNames.Add(assembly.FullName);
                    }
                }
            }
        }

        /// <summary>
        ///     Agrega ensamblados especificos
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assemblyName in AssemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                if (!addedAssemblyNames.Contains(assembly.FullName))
                {
                    assemblies.Add(assembly);
                    addedAssemblyNames.Add(assembly.FullName);
                }
            }
        }

        /// <summary>
        ///     Revisa si  un Dll esta dentro de las no permitidas
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <returns></returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Carga en un lista las dlls disponibles en el directorio bin
        /// </summary>
        /// <param name="directoryPath">La direccion del path.</param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = new List<string>();
            foreach (var a in GetAssemblies())
            {
                loadedAssemblyNames.Add(a.FullName);
            }

            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            foreach (var dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);
                    if (Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                        App.Load(an);
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}