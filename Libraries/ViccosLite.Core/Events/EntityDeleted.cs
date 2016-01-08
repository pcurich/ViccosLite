namespace ViccosLite.Core.Events
{
    public class EntityDeleted<T> where T:BaseEntity
    {
        public T Entity { get; private set; }

        public EntityDeleted(T entity)
        {
            Entity = entity;
        }
    }
}