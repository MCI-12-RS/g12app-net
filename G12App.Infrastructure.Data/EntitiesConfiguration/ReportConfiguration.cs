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

            builder.HasOne(x => x.Cell).WithMany().HasForeignKey(x => x.Cell.Id).IsRequired();
            builder.Navigation(x => x.Cell);
        }
    }
}
