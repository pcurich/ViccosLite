using System.Collections.Generic;
using System.Data.Entity;
using ViccosLite.Core;

namespace ViccosLite.Data.Entities
{
    public interface ISoftContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        int SaveChanges();

        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();

        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false,
            int? timeout = null, params object[] parameters);
    }
}