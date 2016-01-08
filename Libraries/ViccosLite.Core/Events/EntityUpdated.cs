namespace ViccosLite.Core.Events
{
    public class EntityUpdated<T> where T : BaseEntity
    {
        public T Entity { get; private set; }

        public EntityUpdated(T entity)
        {
            Entity = entity;
        }
    }
}