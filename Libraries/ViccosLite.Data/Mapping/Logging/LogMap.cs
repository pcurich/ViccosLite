using ViccosLite.Core.Domain.Logging;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Data.Mapping.Logging
{
    public class LogMap:SoftEntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            ToTable("Log");
            HasKey(o => o.Id);
            Property(o => o.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(o => o.LogLevelId).IsRequired();

            Ignore(o => o.LogLevel);

            HasOptional(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

        }
    }
}