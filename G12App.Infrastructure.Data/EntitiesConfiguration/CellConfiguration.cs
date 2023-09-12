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

            builder.HasOne(x => x.Lead).WithMany().HasForeignKey(x => x.LeadId).IsRequired();
            builder.Navigation(x => x.Lead).AutoInclude();
            builder.HasOne(x => x.Host).WithMany().HasForeignKey(x => x.HostId).IsRequired();
            builder.Navigation(x => x.Host).AutoInclude();
            builder.HasOne(x => x.Timothy).WithMany().HasForeignKey(x => x.TimothyId).IsRequired();
            builder.Navigation(x => x.Timothy).AutoInclude();

            builder.HasOne(x => x.Place).WithMany().HasForeignKey(x => x.PlaceId);
            builder.Navigation(x => x.Place).AutoInclude();
        }
    }
}
