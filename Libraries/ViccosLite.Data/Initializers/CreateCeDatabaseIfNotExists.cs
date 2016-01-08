using System;
using System.Data.Entity;
using System.Transactions;

namespace ViccosLite.Data.Initializers
{
    public class CreateCeDatabaseIfNotExists<TContext> : SqlCeInitializer<TContext> where TContext : DbContext
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

            bool dataBaseExists;

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dataBaseExists = replacedContext.Database.Exists();
            }

            if (dataBaseExists)
            {
                /*
                 * Si no hay ninguna metadata en el modelo o en la dataBase, entonces
                 * se asume que se valido la base de datos porque el caso comun para estos escenarios son 
                 *database/model primero y/o existe dataBase
                 */
                if (!context.Database.CompatibleWithModel(false))
                    throw new InvalidOperationException(
                        string.Format(
                            "El modelo respaldado por '{0}' contexto ha cambiado desde la ultima vez que fue creado " +
                            "Elimine manualmente o actualice la base de datos, o llame al metodo  Database.SetInitializer " +
                            "con una instancia de IDatabaseInitialize." +
                            "Por ejemplo la estrategia DropCreateDatabaseIfModelChanges puede automaticamente borrar " +
                            "y recrear la base de datos, opcionalmente poblarla con nueva data ", context.GetType().Name));
            }
            else
            {
                context.Database.Create();
                Seed(context);
                context.SaveChanges();
            }
        }

        #endregion

        #region Seeding

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