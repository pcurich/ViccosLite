using System;
using System.Data.Entity;

namespace ViccosLite.Data.Initializers
{
    public class DropCreateCeDatabaseAlways<TContext> : SqlCeInitializer<TContext> where TContext : DbContext
    {
        #region Estrategia de implementacion

        /// <summary>
        ///     Ejecuta la estrategia para inicializar la base de datos para el contexto dado
        /// </summary>
        /// <param name="context">El contexto</param>
        public override void InitializeDatabase(TContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var replacedContext = ReplaceSqlCeConnection(context);

            if (replacedContext.Database.Exists())
                replacedContext.Database.Delete();

            context.Database.Create();
            Seed(context);
            context.SaveChanges();
        }

        #endregion

        #region Seeding methods

        /// <summary>
        /// Esto deberia ser sobreescrito para apregar la data en el contexto para
        /// ser insertado 
        /// </summary>
        /// <param name="context">El contexto a poblar</param>
        protected virtual void Seed(TContext context)
        {
        }

        #endregion
    }
}