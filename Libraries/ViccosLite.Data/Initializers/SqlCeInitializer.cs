using System;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.IO;

namespace ViccosLite.Data.Initializers
{
    public abstract class SqlCeInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public abstract void InitializeDatabase(T context);

        #region Helpers

        /// <summary>
        ///     Retorna un nuevo DbContext con el mismo SqlCe connection string
        ///     pero con el  |DataDirectory|  expandido
        /// </summary>
        /// <param name="context">Contexto</param>
        /// <returns>Un nuevo contexto</returns>
        protected static DbContext ReplaceSqlCeConnection(DbContext context)
        {
            if (!(context.Database.Connection is SqlCeConnection))
                return context;

            var builder = new SqlCeConnectionStringBuilder(context.Database.Connection.ConnectionString);
            if (String.IsNullOrWhiteSpace(builder.DataSource))
                return context;

            builder.DataSource = ReplaceDataDirectory(builder.DataSource);
            return new DbContext(builder.ConnectionString);
        }

        private static string ReplaceDataDirectory(string inputString)
        {

            var str = inputString.Trim();
            if (string.IsNullOrEmpty(inputString) ||
                !inputString.StartsWith("|DataDirectory|", StringComparison.InvariantCultureIgnoreCase))
                return str;

            var data = AppDomain.CurrentDomain.GetData("DataDirectory") as string;

            if (string.IsNullOrEmpty(data))
            {
                data = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (string.IsNullOrEmpty(data))
            {
                data = string.Empty;
            }

            var length = "|DataDirectory|".Length;

            if ((inputString.Length > "|DataDirectory|".Length) && ('\\' == inputString["|DataDirectory|".Length]))
            {
                length++;
            }
            return Path.Combine(data, inputString.Substring(length));
        }

        #endregion
    }
}