using Microsoft.EntityFrameworkCore;
using Paynetics.Data.Entities;
using System;

namespace Paynetics.Data
{
    public class PayneticsDbContext : DbContext
    {

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public PayneticsDbContext(DbContextOptions<PayneticsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PayneticsDbContext).Assembly);
        }
    }
}
