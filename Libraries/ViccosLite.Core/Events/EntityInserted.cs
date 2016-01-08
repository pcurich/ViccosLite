namespace ViccosLite.Core.Events
{
    public class EntityInserted<T> where T : BaseEntity
    {
        public T Entity { get; private set; }

        public EntityInserted(T entity)
        {
            Entity = entity;
        }
    }
}