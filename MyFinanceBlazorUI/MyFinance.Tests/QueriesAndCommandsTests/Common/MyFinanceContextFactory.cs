using Microsoft.EntityFrameworkCore;
using MyFinance.Domain;
using MyFinance.Persistence;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Common
{
    public class MyFinanceContextFactory
    {
        public static DataDbContext Create()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new DataDbContext(options);
            context.Database.EnsureCreated();
            context.Categories.AddRange(
                new Category
                {
                    Name = "Category1"
                },
                new Category
                {
                    Name = "Category2"
                },
                new Category
                {
                    Name = "Category3"
                },
                new Category
                {
                    Name = "Category4"
                }
            );
            context.Transactions.AddRange(
                new Transaction
                {
                    TransactionType = Domain.Enums.TransactionType.Income,
                    CategoryId = 1,
                    Name = "Transaction1",
                    Description = "Description1",
                    Sum = 10.10M,
                    DateOfCreation = DateTime.Now
                },
                new Transaction
                {
                    TransactionType = Domain.Enums.TransactionType.Income,
                    CategoryId = 2,
                    Name = "Transaction2",
                    Description = "Description2",
                    Sum = 20.20M,
                    DateOfCreation = DateTime.Now
                },
                new Transaction
                {
                    TransactionType = Domain.Enums.TransactionType.Expenses,
                    CategoryId = 3,
                    Name = "Transaction3",
                    Description = "Description3",
                    Sum = 30.30M,
                    DateOfCreation = DateTime.Now
                },
                new Transaction
                {
                    TransactionType = Domain.Enums.TransactionType.Expenses,
                    CategoryId = 3,
                    Name = "Transaction4",
                    Description = "Description4",
                    Sum = 40.40M,
                    DateOfCreation = DateTime.Now
                },
                new Transaction
                {
                    TransactionType = Domain.Enums.TransactionType.Expenses,
                    CategoryId = 4,
                    Name = "Transaction5",
                    Description = "Description5",
                    Sum = 50.50M,
                    DateOfCreation = DateTime.Now.AddDays(-1)
                });
            context.SaveChanges();
            return context;
        }

        public static void Destroy(DataDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
