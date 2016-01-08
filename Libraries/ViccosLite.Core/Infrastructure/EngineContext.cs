using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using ViccosLite.Core.Configuration;
using ViccosLite.Core.Infrastructure.Patterns;

namespace ViccosLite.Core.Infrastructure
{
    public class EngineContext
    {
        #region Propiedades

        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion

        #region Util

        /// <summary>
        ///     Crea una instancia factories y agrega http application inyectandolas de manera facil
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>Nueva instancia de Engine</returns>
        protected static IEngine CreateEngineInstance(Config config)
        {
            if (config == null || string.IsNullOrEmpty(config.EngineType))
                return new SoftEngine();

            var engineType = Type.GetType(config.EngineType);
            if (engineType == null)
                throw new ConfigurationErrorsException("El tipo '" + config.EngineType +
                                                       "' no se encuentra. Por favor revise la configuracion en  /configuration/soft/engine[@engineType] o revise assemblies perdidos.");
            if (!typeof(IEngine).IsAssignableFrom(engineType))
                throw new ConfigurationErrorsException("El tipo '" + engineType +
                                                       "' no implementa 'Soft.Core.Infrastructure.IEngine' y no puede ser configurado en /configuration/soft/engine[@engineType] para ese proposito.");
            return Activator.CreateInstance(engineType) as IEngine;
        }

        #endregion

        #region Metodos

        /// <summary>
        ///     Inicializa una instacia estatica en el Soft Factory
        /// </summary>
        /// <param name="forceRecreate"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                var config = ConfigurationManager.GetSection("Config") as Config;
                Singleton<IEngine>.Instance = CreateEngineInstance(config);
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        ///     Establece el engine estatico al proveedor de engines.
        ///     Usar este metodo para proveer tu propia implementacion de engine
        /// </summary>
        /// <param name="engine">El Engine a usar</param>
        /// ///
        /// <remarks>Usa este metodo si en verdad sabes lo que haces</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

    }
}