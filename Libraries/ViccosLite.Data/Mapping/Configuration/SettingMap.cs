using ViccosLite.Core.Domain.Configuration;

namespace ViccosLite.Data.Mapping.Configuration
{
    public class SettingMap : SoftEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            ToTable("Setting");
            HasKey(o => o.Id);
            Property(o => o.DateOfControl).IsRequired().HasColumnType("datetime2");
            
            Property(s => s.Name).IsRequired().HasMaxLength(200);
            Property(s => s.Value).IsRequired().HasMaxLength(2000);

        }
    }
}