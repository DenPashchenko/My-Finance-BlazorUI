using Microsoft.EntityFrameworkCore;
using MyFinance.Domain;

namespace MyFinance.Application.Interfaces
{
    public interface IDataDbContext
    {
        DbSet<Category> Categories { get; set; }

        DbSet<Transaction> Transactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}