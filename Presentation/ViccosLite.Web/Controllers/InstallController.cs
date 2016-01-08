using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ViccosLite.Core;
using ViccosLite.Core.Data;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Framework.Controllers;
using ViccosLite.Framework.Security;
using ViccosLite.Services.Installation;
using ViccosLite.Services.Security;
using ViccosLite.Web.Models.Install;

namespace ViccosLite.Web.Controllers
{
    public class InstallController : BasePublicController
    {
        #region Ctr

        public InstallController()
        {
        }

        #endregion

        #region Util

        /// <summary>
        ///     Indica si se usa MARS (Multiple Active Result Sets)
        /// </summary>
        protected bool UseMars
        {
            get { return false; }
        }

        [NonAction]
        protected bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                //Intento de conexion
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        [NonAction]
        protected string CreateDatabase(string connectionString, string collation)
        {
            try
            {
                //Nombre de la base de datos
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;

                //Se crea una cadena de conexion que apunte a la base de datos 'master' Esta siempre existe
                builder.InitialCatalog = "master";
                var masterCatalogConnectionString = builder.ToString();
                var query = string.Format("CREATE DATABASE [{0}]", databaseName);

                if (!String.IsNullOrWhiteSpace(collation))
                    query = string.Format("{0} COLLATE {1}", query, collation);
                using (var conn = new SqlConnection(masterCatalogConnectionString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Se produjo un error al crear la base de datos: {0}", ex.Message);
            }
        }

        [NonAction]
        protected string CreateConnectionString(bool trustedConnection,
            string serverName, string databaseName,
            string userName, string password, int timeout = 0)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.IntegratedSecurity = trustedConnection;
            builder.DataSource = serverName;
            builder.InitialCatalog = databaseName;
            if (!trustedConnection)
            {
                builder.UserID = userName;
                builder.Password = password;
            }
            builder.PersistSecurityInfo = false;
            if (UseMars)
            {
                builder.MultipleActiveResultSets = true;
            }
            if (timeout > 0)
            {
                builder.ConnectTimeout = timeout;
            }
            return builder.ConnectionString;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Inicio de la paguina de instalacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //Establece el tiempo de espera a 5 minutos
            Server.ScriptTimeout = 300;


            var model = new InstallModel
            {
                AdminEmail = "admin@yourStore.com",
                InstallSampleData = false,
                DatabaseConnectionString = "",
                DataProvider = "sqlserver",
                //Servicio de instalacion rapida, no soporta SQL Compact
                DisableSqlCompact =
                    !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseFastInstallationService"]) &&
                    Convert.ToBoolean(ConfigurationManager.AppSettings["UseFastInstallationService"]),
                DisableSampleDataOption =
                    !String.IsNullOrEmpty(ConfigurationManager.AppSettings["DisableSampleDataDuringInstallation"]) &&
                    Convert.ToBoolean(ConfigurationManager.AppSettings["DisableSampleDataDuringInstallation"]),
                SqlAuthenticationType = "sqlauthentication",
                SqlConnectionInfo = "sqlconnectioninfo_values",
                SqlServerCreateDatabase = false,
                UseCustomCollation = false,
                Collation = "SQL_Latin1_General_CP1_CI_AS"
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InstallModel model)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //Establece el tiempo de espera a 5 minutos
            Server.ScriptTimeout = 300;

            if (model.DatabaseConnectionString != null)
                model.DatabaseConnectionString = model.DatabaseConnectionString.Trim();

            model.DisableSqlCompact =
                !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseFastInstallationService"]) &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["UseFastInstallationService"]);
            model.DisableSampleDataOption =
                !String.IsNullOrEmpty(ConfigurationManager.AppSettings["DisableSampleDataDuringInstallation"]) &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["DisableSampleDataDuringInstallation"]);

            //SQL Server
            if (model.DataProvider.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.SqlConnectionInfo.Equals("sqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                {
                    //raw connection string
                    if (string.IsNullOrEmpty(model.DatabaseConnectionString))
                        ModelState.AddModelError("", "Una cadena de conexión SQL se requiere");

                    try
                    {
                        //try to create connection string
                        new SqlConnectionStringBuilder(model.DatabaseConnectionString);
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Mala conexión de SQL cadena de formato");
                    }
                }
                else
                {
                    //Valores
                    if (string.IsNullOrEmpty(model.SqlServerName))
                        ModelState.AddModelError("", "Nombre de SQL Server se requiere");
                    if (string.IsNullOrEmpty(model.SqlDatabaseName))
                        ModelState.AddModelError("", "Nombre de la base de datos se requiere");

                    //Tipo de autenticacion
                    if (model.SqlAuthenticationType.Equals("sqlauthentication", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Autenticacion de SQL
                        if (string.IsNullOrEmpty(model.SqlServerUsername))
                            ModelState.AddModelError("", "Nombre de usuario SQL es necesario");
                        if (string.IsNullOrEmpty(model.SqlServerPassword))
                            ModelState.AddModelError("", "SQL Contraseña del es necesario");
                    }
                }
            }

            //Consider granting access rights to the resource to the ASP.NET request identity. 
            //ASP.NET has a base process identity 
            //(typically {MACHINE}\ASPNET on IIS 5 or Network Service on IIS 6 and IIS 7, 
            //and the configured application pool identity on IIS 7.5) that is used if the application is not impersonating.
            //If the application is impersonating via <identity impersonate="true"/>, 
            //the identity will be the anonymous user (typically IUSR_MACHINENAME) or the authenticated request user.
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //Validar permisos
            var dirsToCheck = FilePermissionHelper.GetDirectoriesWrite(webHelper);
            foreach (var dir in dirsToCheck)
                if (!FilePermissionHelper.CheckPermissions(dir, false, true, true, false))
                    ModelState.AddModelError("", string.Format("El '{0}' cuenta no se concede permiso para modificar la carpeta '{1}'. Por favor, configurar estos permisos", WindowsIdentity.GetCurrent().Name, dir));

            var filesToCheck = FilePermissionHelper.GetFilesWrite(webHelper);
            foreach (var file in filesToCheck)
                if (!FilePermissionHelper.CheckPermissions(file, false, true, true, true))
                    ModelState.AddModelError("", string.Format("El '{0}' cuenta no se concede permiso para modificar el archivo '{1}'. Por favor, configurar estos permisos", WindowsIdentity.GetCurrent().Name, file));

            if (ModelState.IsValid)
            {
                var settingsManager = new DataSettingsManager();
                try
                {
                    string connectionString;
                    if (model.DataProvider.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //SQL Server
                        if (model.SqlConnectionInfo.Equals("sqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //Cadena de conexion en bruto

                            //Sabemos que la opcion MARS  es requerida cuando se usa entity framework
                            //nos aseguraremos que este especificada
                            var sqlCsb = new SqlConnectionStringBuilder(model.DatabaseConnectionString);
                            if (UseMars)
                            {
                                sqlCsb.MultipleActiveResultSets = true;
                            }
                            connectionString = sqlCsb.ToString();
                        }
                        else
                        {
                            //values
                            connectionString = CreateConnectionString(model.SqlAuthenticationType == "windowsauthentication",
                                    model.SqlServerName, model.SqlDatabaseName,
                                    model.SqlServerUsername, model.SqlServerPassword);
                        }

                        if (model.SqlServerCreateDatabase)
                        {
                            if (!SqlServerDatabaseExists(connectionString))
                            {
                                //create database
                                var collation = model.UseCustomCollation ? model.Collation : "";
                                var errorCreatingDatabase = CreateDatabase(connectionString, collation);
                                if (!String.IsNullOrEmpty(errorCreatingDatabase))
                                    throw new Exception(errorCreatingDatabase);

                                //La base de datos no ha podido ser creada. 
                                //RARO! Parece ser un tema de Entity Framework
                                //Esperamos 10 segundos
                                Thread.Sleep(10000);
                            }
                        }
                        else
                        {
                            //Verificamos que la base de datos exista
                            if (!SqlServerDatabaseExists(connectionString))
                                throw new Exception("Base de datos no existe o no tiene permisos para conectarse a ella");
                        }
                    }
                    else
                    {
                        //SQL CE
                        const string DATABASE_FILE_NAME = "ViccosLite.Db.sdf";
                        const string DATABASE_PATH = @"|DataDirectory|\" + DATABASE_FILE_NAME;
                        connectionString = "Data Source=" + DATABASE_PATH + ";Persist Security Info=False";

                        //drop database if exists
                        var databaseFullPath = HostingEnvironment.MapPath("~/App_Data/") + DATABASE_FILE_NAME;
                        if (System.IO.File.Exists(databaseFullPath))
                        {
                            System.IO.File.Delete(databaseFullPath);
                        }
                    }

                    //save settings
                    var dataProvider = model.DataProvider;
                    var settings = new DataSettings
                    {
                        DataProvider = dataProvider,
                        DataConnectionString = connectionString
                    };
                    settingsManager.SaveSettings(settings);

                    //Inicia proveedor de la Data
                    var dataProviderInstance = EngineContext.Current.Resolve<BaseDataProviderManager>().LoadDataProvider();
                    dataProviderInstance.InitDatabase();


                    //Resolvemos el servicio de instalacion
                    var installationService = EngineContext.Current.Resolve<IInstallationService>();
                    installationService.InstallData(model.AdminEmail, model.AdminPassword, model.InstallSampleData);

                    //Reseteamos la cache
                    DataSettingsHelper.ResetCache();

                  

                    //Registro de los permisos por default
                    var permissionProviders = new List<Type>();
                    permissionProviders.Add(typeof(StandardPermissionProvider));
                    foreach (var providerType in permissionProviders)
                    {
                        dynamic provider = Activator.CreateInstance(providerType);
                        EngineContext.Current.Resolve<IPermissionService>().InstallPermissions(provider);
                    }

                    //Reinicia la aplicacion
                    webHelper.RestartAppDomain();

                    //Redireccion a Home
                    return RedirectToRoute("HomePage");
                }
                catch (Exception exception)
                {
                    //reset cache
                    DataSettingsHelper.ResetCache();

                    //clear provider settings if something got wrong
                    settingsManager.SaveSettings(new DataSettings
                    {
                        DataProvider = null,
                        DataConnectionString = null
                    });

                    ModelState.AddModelError("",
                        string.Format("Error de instalación: {0}", exception.Message));
                }
            }
            return View(model);
        }


        /// <summary>
        /// Reinicia la aplicacion redireccionandola a Home o a Install
        /// </summary>
        /// <returns></returns>
        public ActionResult RestartInstall()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //reinicia la aplicacion
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            webHelper.RestartAppDomain();

            //Redirecciona la pagina
            return RedirectToAction("Index", "Install");
            //return RedirectToRoute("HomePage");
        }

        #endregion
    }
}