using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ViccosLite.Core;
using ViccosLite.Data.Entities;

namespace ViccosLite.Data.Extensions
{
    public static class DbContextExtensions
    {
        #region Util

        private static T InnerGetCopy<T>(ISoftContext context, T currentCopy,
            Func<DbEntityEntry<T>, DbPropertyValues> func) where T : BaseEntity
        {
            //Se optiene el contexto de base de datos
            var dbContext = CastOrThrow(context);

            //Obtiene el objeto entity de seguimiento
            var entry = GetEntityOrReturnNull(currentCopy, dbContext);

            //La salida
            T output = null;

            //Intenta optener los valores
            if (entry != null)
            {
                DbPropertyValues dbPropertyValues = func(entry);
                if (dbPropertyValues != null)
                {
                    output = dbPropertyValues.ToObject() as T;
                }
            }
            return output;
        }

        /// <summary>
        /// Me da el entity o me retorna nulo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentCopy">La actual copia</param>
        /// <param name="dbContext">el contexto de bd</param>
        /// <returns></returns>
        private static DbEntityEntry<T> GetEntityOrReturnNull<T>(T currentCopy, DbContext dbContext)
            where T : BaseEntity
        {
            return dbContext.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == currentCopy);
        }

        private static DbContext CastOrThrow(ISoftContext context)
        {
            var output = context as DbContext;

            if (output == null)
                throw new InvalidOperationException("El contexto no soporta esta operacion. No es una instancia de DbContext");

            return output;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Carga la copia original
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="currentCopy"></param>
        /// <returns></returns>
        public static T LoadOriginalCopy<T>(this ISoftContext context, T currentCopy) where T : BaseEntity
        {
            return InnerGetCopy(context, currentCopy, e => e.OriginalValues);
        }

        /// <summary>
        /// Carga una copia de la base de datos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="currentCopy"></param>
        /// <returns></returns>
        public static T LoadDatabaseCopy<T>(this ISoftContext context, T currentCopy) where T : BaseEntity
        {
            return InnerGetCopy(context, currentCopy, e => e.GetDatabaseValues());
        }

        /// <summary>
        /// Borra una tabla de plugin
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tableName"></param>
        public static void DropPluginTable(this DbContext context, string tableName)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            //Borrar la tabla
            if (
                context.Database.SqlQuery<int>("SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}",
                    tableName).Any<int>())
            {
                var dbScript = "DROP TABLE [" + tableName + "]";
                context.Database.ExecuteSqlCommand(dbScript);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Obtiene el nombre de la tabla del entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetTableName<T>(this ISoftContext context) where T : BaseEntity
        {
            //var tableName = typeof(T).Name;
            //return tableName;

            //Este codigo solo funciona con entity
            //Si se quiere usar otra base de datos usar el metodo de arriba

            var adapter = ((IObjectContextAdapter)context).ObjectContext;
            var storageModel = (StoreItemCollection)adapter.MetadataWorkspace.GetItemCollection(DataSpace.SSpace);
            var containers = storageModel.GetItems<EntityContainer>();
            var entitySetBase =
                containers.SelectMany(c => c.BaseEntitySets.Where(bes => bes.Name == typeof(T).Name)).First();

            //Estas son las variables que contendrán tabla en el nombre del esquema
            var tableName = entitySetBase.MetadataProperties.First(p => p.Name == "Table").Value.ToString();
            //var schemaName = entitySetBase.MetadataProperties.First(p => p.Name == "Schema").Value.ToString();
            return tableName;
        }

        /// <summary>
        /// Optiene la maxima longitud de una columna
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int? GetColumnMaxLength(this ISoftContext context, string entityTypeName, string columnName)
        {
            //original: http://stackoverflow.com/questions/5081109/entity-framework-4-0-automatically-truncate-trim-string-before-insert
            int? result = null;

            var entType = Type.GetType(entityTypeName);
            var adapter = ((IObjectContextAdapter)context).ObjectContext;
            var metadataWorkspace = adapter.MetadataWorkspace;
            var q =
                from meta in
                    metadataWorkspace.GetItems(DataSpace.CSpace)
                        .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                from p in
                    (meta as EntityType).Properties.Where(
                        p => p.Name == columnName && p.TypeUsage.EdmType.Name == "String")
                select p;

            var queryResult = q.Where(p =>
            {
                var match = p.DeclaringType.Name == entityTypeName;
                if (!match && entType != null)
                {
                    //Is a fully qualified name....
                    match = entType.Name == p.DeclaringType.Name;
                }

                return match;
            }).Select(sel => sel.TypeUsage.Facets["MaxLength"].Value);

            var enumerable = queryResult as IList<object> ?? queryResult.ToList();
            if (enumerable.Any())
            {
                result = Convert.ToInt32(enumerable.First());
            }

            return result;
        }

        #endregion
    }
}