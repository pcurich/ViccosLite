using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Hosting;
using ViccosLite.Core.Data;
using ViccosLite.Data.Entities;
using ViccosLite.Data.Initializers;

namespace ViccosLite.Data.Provider
{
    public class SqlServerDataProvider : IDataProvider
    {
        #region Util

        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("El archivo  - {0} no existe", filePath));

                return new string[0];
            }


            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }

            return statements.ToArray();
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    return sb.Length > 0 ? sb.ToString() : null;
                }

                if ("GO".Equals(lineOfText.TrimEnd().ToUpper()))
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Metodos

        /// <summary>
        ///     Inicializa las conexiones
        /// </summary>
        public virtual void InitConnectionFactory()
        {
            var connectionFactory = new SqlConnectionFactory();
            #pragma warning disable 0618
            Database.DefaultConnectionFactory = connectionFactory;
            //@todo http://www.entityframeworktutorial.net/EntityFramework6/code-based-configuration.aspx
        }

        /// <summary>
        ///     Establece el inicio de la base de datos
        /// </summary>
        public virtual void SetDatabaseInitializer()
        {
            //Paso algunas tablas a buscar
            var tablesToValidate = new[] { "User" };

            //Comandos personalizados (StoreProcedure, indexes)
            var customCommands = new List<string>();
            customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Install/SqlServer.Indexes.sql"), false));
            customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Install/SqlServer.StoredProcedures.sql"), false));

            var initializer = new CreateTablesIfNotExist<SoftContext>(tablesToValidate, customCommands.ToArray());
            Database.SetInitializer(initializer);
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
        /// Indica si el proveedor de base de datos soporta storeProcedures0
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return true; }
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

        #endregion
    }
}