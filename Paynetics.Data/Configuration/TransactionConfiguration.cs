using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paynetics.Data.Entities;

namespace Paynetics.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();
            builder.Property(b => b.BeneficiaryIban).HasMaxLength(34).IsRequired();
            builder.Property(b => b.DebtorIban).HasMaxLength(34).IsRequired();
            builder.Property(x => x.Status).HasConversion<int>();

        }
    }
}
