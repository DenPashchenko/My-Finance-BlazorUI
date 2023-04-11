using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Transactions.Commands.UpdateTransaction;
using MyFinance.Domain.Enums;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;
using System.ComponentModel;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Transactions.CommandTests
{
    public class UpdateTransactionCommandHandlerTests : TestFixtureBase
    {
        [Fact]
        public async Task UpdateTransactionCommandHandler_ValidData_Success()
        {
            var handler = new UpdateTransactionCommandHandler(_context);
            int idToUpdate = 5;
            var transactionType = TransactionType.Income;
            var categoryId = 1;
            var transactionName = "Updated transaction";
            var description = "Updated description";
            var sum = 222.22M;

            await handler.Handle(
                new UpdateTransactionCommand
                {
                    Id = idToUpdate,
                    TransactionType = transactionType,
                    CategoryId = categoryId,
                    Name = transactionName,
                    Description = description,
                    Sum = sum
                },
                CancellationToken.None);

            Assert.NotNull(await _context.Transactions.SingleOrDefaultAsync(transaction =>
                transaction.Id == idToUpdate &&
                transaction.TransactionType == transactionType &&
                transaction.Name == transactionName &&
                transaction.CategoryId == categoryId &&
                transaction.Description == description &&
                transaction.Sum == sum &&
                transaction.DateOfCreation.Date == DateTime.Now.AddDays(-1).Date &&
                transaction.DateOfEditing.Value.Date == DateTime.Now.Date));
        }

        [Fact]
        public async Task UpdateTransactionCommandHandler_InvalidId_NotFoundException()
        {
            var handler = new UpdateTransactionCommandHandler(_context);
            int invalidId = 0;
            var transactionType = TransactionType.Income;
            var categoryId = 1;
            var transactionName = "Updated transaction";
            var description = "Updated description";
            var sum = 222.22M;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(
                   new UpdateTransactionCommand
                   {
                       Id = invalidId,
                       TransactionType = transactionType,
                       CategoryId = categoryId,
                       Name = transactionName,
                       Description = description,
                       Sum = sum
                   },
                CancellationToken.None));
        }

        [Fact]
        public async Task UpdateTransactionCommandHandler_InvalidCategoryId_NotFoundException()
        {
            var handler = new UpdateTransactionCommandHandler(_context);
            int idToUpdate = 1;
            var transactionType = TransactionType.Income;
            var invalidCategoryId = 0;
            var transactionName = "Updated transaction";
            var description = "Updated description";
            var sum = 222.22M;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(
                   new UpdateTransactionCommand
                   {
                       Id = idToUpdate,
                       TransactionType = transactionType,
                       CategoryId = invalidCategoryId,
                       Name = transactionName,
                       Description = description,
                       Sum = sum
                   },
                CancellationToken.None));
        }

        [Fact]
        public async Task UpdateTransactionCommandHandler_InvalidTransactionType_InvalidEnumArgumentException()
        {
            var handler = new UpdateTransactionCommandHandler(_context);
            int idToUpdate = 1;
            var invalidType = 3;
            var categoryId = 1;
            var transactionName = "Updated transaction";
            var description = "Updated description";
            var sum = 222.22M;

            await Assert.ThrowsAsync<InvalidEnumArgumentException>(async () =>
               await handler.Handle(
                   new UpdateTransactionCommand
                   {
                       Id = idToUpdate,
                       TransactionType = (TransactionType)invalidType,
                       CategoryId = categoryId,
                       Name = transactionName,
                       Description = description,
                       Sum = sum
                   },
                CancellationToken.None));
        }
    }
}
