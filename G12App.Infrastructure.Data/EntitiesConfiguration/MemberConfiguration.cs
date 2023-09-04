using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.EntitiesConfiguration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();

            // SEED
            builder.HasData(new Member() { Id = 1, Name = "João" });
            builder.HasData(new Member() { Id = 2, Name = "Judas" });
            builder.HasData(new Member() { Id = 3, Name = "Tomé" });
            builder.HasData(new Member() { Id = 4, Name = "Felipe" });
        }
    }
}
