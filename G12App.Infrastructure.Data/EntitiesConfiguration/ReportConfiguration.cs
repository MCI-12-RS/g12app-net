using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G12App.Infrastructure.Data.EntitiesConfiguration
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(x => x.ReportId);
            builder.HasMany(x => x.Members).WithMany().UsingEntity(
                x =>
                {
                    x.Property("MemberId").HasColumnName("MemberForeingKey");
                    x.Property("ReportId").HasColumnName("ReportForeingKey");
                });

        }
    }
}
