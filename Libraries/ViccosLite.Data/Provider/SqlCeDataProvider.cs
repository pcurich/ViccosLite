using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using ViccosLite.Core.Data;
using ViccosLite.Data.Entities;
using ViccosLite.Data.Initializers;

namespace ViccosLite.Data.Provider
{
    public class SqlCeDataProvider : IDataProvider
    {
        /// <summary>
        /// Inicializa las conexiones
        /// </summary>
        public virtual void InitConnectionFactory()
        {
            var connectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            //TODO fix compilation warning (below)
            #pragma warning disable 0618
            Database.DefaultConnectionFactory = connectionFactory;
        }

        /// <summary>
        /// Inicializa la base de datos
        /// </summary>
        public virtual void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
        }

        /// <summary>
        /// Establece el inicio de la base de datos
        /// </summary>
        public virtual void SetDatabaseInitializer()
        {
            var initializer = new CreateCeDatabaseIfNotExists<SoftContext>();
            Database.SetInitializer(initializer);
        }

        /// <summary>
        /// Indica si el proveedor de base de datos soporta storeProcedures0
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return false; }
        }

        /// <summary>
        /// Establece el soporte para los parametros de la base de datos
        /// (solo se usan en storeProcedure)
        /// </summary>
        /// <returns></returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }  
    }
}