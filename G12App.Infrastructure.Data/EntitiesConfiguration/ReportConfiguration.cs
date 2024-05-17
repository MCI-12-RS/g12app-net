using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G12App.Infrastructure.Data.EntitiesConfiguration
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Members).WithMany(x => x.Reports);
        }
    }
}
