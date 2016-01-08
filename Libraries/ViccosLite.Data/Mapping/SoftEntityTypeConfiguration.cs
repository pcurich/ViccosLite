using System.Data.Entity.ModelConfiguration;

namespace ViccosLite.Data.Mapping
{
    public abstract class SoftEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected SoftEntityTypeConfiguration()
        {
            PostInitialize();
        }

        protected void PostInitialize()
        {
        }
    }
}