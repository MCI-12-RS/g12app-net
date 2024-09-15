using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G12App.Infrastructure.Data.EntitiesConfiguration
{
    public class CellConfiguration : IEntityTypeConfiguration<Cell>
    {
        public void Configure(EntityTypeBuilder<Cell> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Lead).WithMany(x => x.CellsLeading).HasForeignKey(x => x.LeadId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Host).WithMany(x => x.CellsHosting).HasForeignKey(x => x.HostId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Timothy).WithMany(x => x.CellsServing).HasForeignKey(x => x.TimothyId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Place).WithMany().HasForeignKey(x => x.PlaceId).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
