using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G12App.Infrastructure.Data.EntitiesConfiguration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.HomeAddress).WithMany().HasForeignKey(x => x.HomeAddressId);

            builder.HasOne(x => x.Leadership).WithMany().HasForeignKey(x => x.LeadershipId);

            builder.HasOne(a => a.Spouse).WithOne()
                .HasForeignKey<Member>(b => b.SpouseId).IsRequired(false)
                .HasPrincipalKey<Member>(x => x.Id).IsRequired(false);
        }
    }
}
