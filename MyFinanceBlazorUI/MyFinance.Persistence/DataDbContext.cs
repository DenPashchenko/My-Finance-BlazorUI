using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Interfaces;
using MyFinance.Domain;
using MyFinance.Persistence.EntityTypeConfigurations;

namespace MyFinance.Persistence
{
    public class DataDbContext : DbContext, IDataDbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
