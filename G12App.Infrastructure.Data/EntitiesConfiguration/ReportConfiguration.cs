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

            builder.HasMany(x => x.Members).WithMany(x => x.Reports).UsingEntity<ReportMember>();

            builder.HasOne(x => x.Lead).WithMany().HasForeignKey(x => x.LeadershipId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Host).WithMany().HasForeignKey(x => x.HostId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Timothy).WithMany().HasForeignKey(x => x.TimothyId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Cell).WithMany().HasForeignKey(x => x.CellId).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
