using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using ViccosLite.Core.Configuration;

namespace ViccosLite.Core.Infrastructure
{
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Propiedades

        private bool _binFolderAssembliesLoaded;

        /// <summary>
        /// Establece u optiene cualquier ensamblado en el folder bin de la aplicacion
        /// Deberia estar especificado para empezar a ser cargado en la carga de la aplicacion.
        /// Esto es necesario en situaciones donde los plugins necesiten ser cargados
        /// en la aplicacion despues sera recargada
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded { get; set; }

        #endregion

        #region Ctr

        public WebAppTypeFinder(Config config)
        {
            EnsureBinFolderAssembliesLoaded = config.DynamicDiscovery;
        }

        #endregion

        #region Metodos

        public virtual string GetBinDirectory()
        {
            return HostingEnvironment.IsHosted
                ? HttpRuntime.BinDirectory
                : AppDomain.CurrentDomain.BaseDirectory;
        }

        public override IList<Assembly> GetAssemblies()
        {
            if (EnsureBinFolderAssembliesLoaded && !_binFolderAssembliesLoaded)
            {
                _binFolderAssembliesLoaded = true;
                var binPath = GetBinDirectory();
                //binPath = _webHelper.MapPath("~/bin");
                LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }

        #endregion
    }
}