using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using ViccosLite.Core.Data;
using ViccosLite.Core.Infrastructure;

namespace ViccosLite.Core
{
    public  class WebHelper : IWebHelper
    {
        #region Propiedades

        private readonly HttpContextBase _httpContext;

        #endregion

        #region Utilities

        protected virtual Boolean IsRequestAvailable(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            try
            {
                if (httpContext.Request == null)
                    return false;
            }
            catch (HttpException)
            {
                return false;
            }

            return true;
        }

        protected virtual bool TryWriteWebConfig()
        {
            try
            {
                //En un medio de confianza, "UnloadAppDomain" no es soportado. Manipulamos we.config
                //para forzas un reinicio de un AppDomain
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual bool TryWriteGlobalAsax()
        {
            try
            {
                //Cuando un nuevo plugin es colocado en la carpeta de plugis y este es instalado
                //incluso si el plugin ha sido registrado sus rutas por los controladores, 
                //entonces las rutas no trabajaran con el framework MVC no pudiendo 
                //encontrar el nuevo typo de controlador y no puede instanciarse 
                //para estos errores 
                //i.e. controller no ha implementado IController
                //Este problema se describe en  http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                //La solucion esta en tocar el archivo global.asax
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Methods

        public WebHelper(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public virtual string GetUrlReferrer()
        {
            var referrerUrl = string.Empty;

            //referencia al URL es nula en algunos casos (xe en IE 8)
            if (IsRequestAvailable(_httpContext) &&
                _httpContext.Request.UrlReferrer != null)

                referrerUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        public virtual string GetCurrentIpAddress()
        {
            var result = string.Empty;

            if (!IsRequestAvailable(_httpContext))
                return result;

            if (_httpContext.Request.Headers != null)
            {
                //La cabecera http X-Forwarded-For (XFF) es un estandar
                //para identificar la ip origen del cliente
                //que se conecta al servidor a traves de un proxy http o por balanceador de carga
                var forwardedHttpHeader = "X-FORWARDED-FOR";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ForwardedHTTPheader"]))
                {
                    //pero en otros casos el el servidor una otras cabeceras http
                    //en esos casos un administrador puede especificar una cabecera HTTP Forwarded
                    forwardedHttpHeader = ConfigurationManager.AppSettings["ForwardedHTTPheader"];
                }

                //Esto es usado para identificar el origen de la direccion IP de un cliente 
                //que se conecta al servidor a traves de un proxy http o por balanceador de carga

                var xff = _httpContext.Request.Headers.AllKeys
                    .Where(x => forwardedHttpHeader.Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => _httpContext.Request.Headers[k])
                    .FirstOrDefault();

                //si se quiere excluir Ip privadas  ver http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc

                if (!String.IsNullOrEmpty(xff))
                {
                    result = xff.Split(new[] { ',' }).FirstOrDefault();
                }

                if (String.IsNullOrEmpty(result) && _httpContext.Request.UserHostAddress != null)
                {
                    result = _httpContext.Request.UserHostAddress;
                }

                //algunas validaciones
                if (result == "::1")
                    result = "127.0.0.1";

                //remover puerto
                if (String.IsNullOrEmpty(result))
                    return result;

                var index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    result = result.Substring(0, index);
            }
            return result;
        }

        public virtual string GetThisPageUrl(bool includeQueryString)
        {
            var useSsl = IsCurrentConnectionSecured();
            return GetThisPageUrl(includeQueryString, useSsl);
        }

        public virtual string GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            var url = string.Empty;
            if (!IsRequestAvailable(_httpContext))
                return url;

            if (includeQueryString)
            {
                var storeHost = GetStoreHost(useSsl);
                if (storeHost.EndsWith("/"))
                    storeHost = storeHost.Substring(0, storeHost.Length - 1);
                url = storeHost + _httpContext.Request.RawUrl;
            }
            else
            {
                if (_httpContext.Request.Url != null)
                {
                    url = _httpContext.Request.Url.GetLeftPart(UriPartial.Path);
                }
            }
            url = url.ToLowerInvariant();
            return url;
        }


        public virtual bool IsCurrentConnectionSecured()
        {
            var useSsl = false;
            if (IsRequestAvailable(_httpContext))
            {
                useSsl = _httpContext.Request.IsSecureConnection;
                //cuando el host usa un balanceador de carga en su server entonces el  Request.IsSecureConnection  nunca es seteado a verdadero se usa de la siguiente manera
                //usa este en caso de que sea
                //useSSL = _httpContext.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSsl;
        }

        public virtual string ServerVariables(string name)
        {
            var result = string.Empty;

            try
            {
                if (!IsRequestAvailable(_httpContext))
                    return result;

                //poner en  try-catch 
                //se describe aqui http://www.nopcommerce.com/boards/t/21356/multi-store-roadmap-lets-discuss-update-done.aspx?p=6#90196
                if (_httpContext.Request.ServerVariables[name] != null)
                {
                    result = _httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        public virtual string GetStoreHost(bool useSsl)
        {
            var result = string.Empty;
            var httpHost = ServerVariables("HTTP_HOST");

            if (!String.IsNullOrEmpty(httpHost))
            {
                result = "http://" + httpHost;
                if (!result.EndsWith("/"))
                    result += "/";
            }


            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                #region Database is installed

                //Dejar que IWorkContext resuelva esto
                //No se hace una inyeccion por el contructor 
                //porque ocasionaria una referencia circular
                var storeContext = EngineContext.Current.Resolve<IStoreContext>();
                var currentStore = storeContext.CurrentStore;
                if (currentStore == null)
                    throw new Exception("Current store cannot be loaded");

                if (String.IsNullOrWhiteSpace(httpHost))
                {
                    //variable HTTP_HOST no esta.
                    //esto sucede cuando el HttpContext no esta disponible 
                    //x ejemplo cuando esta corriendo en una tarea programada
                    //En este caso se usa el URL  de un entity de tienda configurado en e larea de admin
                    result = currentStore.Url;
                    if (!result.EndsWith("/"))
                        result += "/";
                }

                if (useSsl)
                {
                    if (!String.IsNullOrWhiteSpace(currentStore.SecureUrl))
                    {
                        //URl especifica segura 
                        //entonces el dueño de la tienda no desea 
                        //que sea detectado automaticamente
                        //En este caso se usa el URL seguro especifico 
                        result = currentStore.SecureUrl;
                    }
                    else
                    {
                        //un URL seguro no ha sido especificado 
                        //entonces el dueño debe ser detectado automaticamente
                        result = result.Replace("http:/", "https:/");
                    }

                }
                if (currentStore.SslEnabled && !String.IsNullOrWhiteSpace(currentStore.SecureUrl))
                {

                    //el SSL esta activo en la tienda y con url segura 
                    //entonces el dueño de la tienda no quiere que sea detectado
                    //automaticamente en este caso se usa un URL no segura
                    result = currentStore.Url;
                }

                #endregion
            }
            else
            {
                #region Database is not installed
                if (useSsl)
                {
                    //URL seguro no ha sido especificado
                    //Entonces el sueño de una tienda qioere que sea detectado automaticamente
                    result = result.Replace("http:/", "https:/");
                }
                #endregion
            }

            if (!result.EndsWith("/"))
                result += "/";
            return result.ToLowerInvariant();
        }

        public virtual string GetStoreLocation()
        {
            var useSsl = IsCurrentConnectionSecured();
            return GetStoreLocation(useSsl);
        }

        public virtual string GetStoreLocation(bool useSsl)
        {
            //return HostingEnvironment.ApplicationVirtualPath;

            var result = GetStoreHost(useSsl);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            if (IsRequestAvailable(_httpContext))
                result = result + _httpContext.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        public virtual bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var path = request.Path;
            var extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //no host. prueba unitaria
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        public virtual string ModifyQueryString(string url, string queryStringModification, string anchor)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (queryStringModification == null)
                queryStringModification = string.Empty;
            queryStringModification = queryStringModification.ToLowerInvariant();

            if (anchor == null)
                anchor = string.Empty;
            anchor = anchor.ToLowerInvariant();

            var str = string.Empty;
            var str2 = string.Empty;

            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#", StringComparison.Ordinal) + 1);
                url = url.Substring(0, url.IndexOf("#", StringComparison.Ordinal));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?", StringComparison.Ordinal) + 1);
                url = url.Substring(0, url.IndexOf("?", StringComparison.Ordinal));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (var str3 in str.Split(new[] { '&' }))
                    {
                        if (string.IsNullOrEmpty(str3))
                            continue;

                        var strArray = str3.Split(new[] { '=' });
                        if (strArray.Length == 2)
                        {
                            if (!dictionary.ContainsKey(strArray[0]))
                            {
                                //No agregue el valor si es que existe
                                //2 en el mismo query como parametro? toricamente es imposible
                                //pero MVC  tiene una feas implementaciones para chekboxesy posiblemente se tiene 2 valores
                                //mas info aca: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
                                //se hace estavalidacion para asegurar que el primero no se valla a sobreescribir
                                dictionary[strArray[0]] = strArray[1];
                            }
                        }
                        else
                        {
                            dictionary[str3] = null;
                        }
                    }
                    foreach (var str4 in queryStringModification.Split(new[] { '&' }))
                    {
                        if (string.IsNullOrEmpty(str4))
                            continue;

                        var strArray2 = str4.Split(new[] { '=' });
                        if (strArray2.Length == 2)
                        {
                            dictionary[strArray2[0]] = strArray2[1];
                        }
                        else
                        {
                            dictionary[str4] = null;
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (var str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] == null)
                            continue;

                        builder.Append("=");
                        builder.Append(dictionary[str5]);
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(anchor))
            {
                str2 = anchor;
            }
            return
                (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2)))
                    .ToLowerInvariant();
        }

        public virtual string RemoveQueryString(string url, string queryString)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (queryString == null)
                queryString = string.Empty;
            queryString = queryString.ToLowerInvariant();


            var str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?", StringComparison.Ordinal) + 1);
                url = url.Substring(0, url.IndexOf("?", StringComparison.Ordinal));
            }
            if (string.IsNullOrEmpty(queryString))
                return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));

            if (string.IsNullOrEmpty(str))
                return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));

            var dictionary = new Dictionary<string, string>();
            foreach (var str3 in str.Split(new[] { '&' }))
            {
                if (string.IsNullOrEmpty(str3))
                    continue;

                var strArray = str3.Split(new[] { '=' });
                if (strArray.Length == 2)
                {
                    dictionary[strArray[0]] = strArray[1];
                }
                else
                {
                    dictionary[str3] = null;
                }
            }
            dictionary.Remove(queryString);

            var builder = new StringBuilder();
            foreach (var str5 in dictionary.Keys)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }

                builder.Append(str5);
                if (dictionary[str5] == null)
                    continue;

                builder.Append("=");
                builder.Append(dictionary[str5]);
            }
            str = builder.ToString();
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        public virtual T QueryString<T>(string name)
        {
            string queryParam = null;

            if (IsRequestAvailable(_httpContext) && _httpContext.Request.QueryString[name] != null)
                queryParam = _httpContext.Request.QueryString[name];

            return !String.IsNullOrEmpty(queryParam) ? CommonHelper.To<T>(queryParam) : default(T);
        }

        public virtual void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "")
        {
            if (CommonHelper.GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
            {
                //full trust
                HttpRuntime.UnloadAppDomain();

                TryWriteGlobalAsax();
            }
            else
            {
                //medium trust
                var success = TryWriteWebConfig();
                if (!success)
                {
                    throw new KsException(
                        "soft needs to be restarted due to a configuration change, but was unable to do so." +
                        Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" +
                        Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'web.config' file.");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new KsException(
                        "soft needs to be restarted due to a configuration change, but was unable to do so." +
                        Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" +
                        Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'Global.asax' file.");
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            if (_httpContext != null && makeRedirect)
            {
                if (String.IsNullOrEmpty(redirectUrl))
                    redirectUrl = GetThisPageUrl(true);
                _httpContext.Response.Redirect(redirectUrl, true /*endResponse*/);
            }
        }

        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = _httpContext.Response;
                return response.IsRequestBeingRedirected;
            }
        }

        public virtual bool IsPostBeingDone
        {
            get
            {
                return _httpContext.Items["soft.IsPOSTBeingDone"] != null &&
                       Convert.ToBoolean(_httpContext.Items["soft.IsPOSTBeingDone"]);
            }
            set { _httpContext.Items["soft.IsPOSTBeingDone"] = value; }
        }

        #endregion
    }
}