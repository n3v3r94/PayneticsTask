using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paynetics.Data.Entities;
using System.Reflection.Emit;

namespace Paynetics.Data.Configuration
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.HasKey(x=> x.Id);   
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.HasMany(x => x.Transactions).WithOne(x => x.Merchant).HasForeignKey(x => x.MerchantId);
            builder.Property(m => m.Id).UseHiLo("MerchantSequence");
        }
    }
}
