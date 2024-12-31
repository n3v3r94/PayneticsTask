using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paynetics.Data.Entities;

namespace Paynetics.Data.Configuration
{
    public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.HasMany(x => x.Merchants).WithOne(x => x.Partner).HasForeignKey(x => x.PartnerId);
            builder.Property(p => p.Id).UseHiLo("PartnerSequence");
        }
    }
}
