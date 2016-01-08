using System;
using System.Data.Entity;
using System.Transactions;

namespace ViccosLite.Data.Initializers
{
    public class DropCreateCeDatabaseIfModelChanges<TContext> : SqlCeInitializer<TContext> where TContext : DbContext
    {
        #region Strategy implementation

        /// <summary>
        ///     Ejecuta la estrategia para inicializar la base de datos para el contexto dado
        /// </summary>
        /// <param name="context">El contexto</param>
        public override void InitializeDatabase(TContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var replacedContext = ReplaceSqlCeConnection(context);

            bool databaseExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
                databaseExists = replacedContext.Database.Exists();

            if (databaseExists)
            {
                if (context.Database.CompatibleWithModel(true))
                    return;
                replacedContext.Database.Delete();
            }

            // Database no existe o fue borrada, entonces se recreara otra vez.
            context.Database.Create();
            Seed(context);
            context.SaveChanges();
        }

        #endregion

        #region Seeding

        /// <summary>
        ///     Esto deberia ser sobreescrito para apregar la data en el contexto para
        ///     ser insertado
        /// </summary>
        /// <param name="context">El contexto a poblar</param>
        protected virtual void Seed(TContext context)
        {
        }

        #endregion
    }
}