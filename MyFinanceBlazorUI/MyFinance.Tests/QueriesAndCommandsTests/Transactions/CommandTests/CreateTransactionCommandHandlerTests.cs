using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Transactions.Commands.CreateTransaction;
using MyFinance.Application.Transactions.Queries.GetTransactionById;
using MyFinance.Domain.Enums;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;
using System.ComponentModel;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Transactions.CommandTests
{
    public class CreateTransactionCommandHandlerTests : TestFixtureBase
    {
        [Fact]
        public async Task CreateTransactionCommandHandler_ValidData_Success()
        {
            var handler = new CreateTransactionCommandHandler(_context, _mapper);
            var transactionType = TransactionType.Income;
            var categoryId = 1;
            var transactionName = "New transaction";
            var description = "New description";
            var sum = 11.11M;

            var result = await handler.Handle(
                new CreateTransactionCommand
                {
                    TransactionType = transactionType,
                    CategoryId = categoryId,
                    Name = transactionName,
                    Description = description,
                    Sum = sum
                },
                CancellationToken.None);

            Assert.NotNull(await _context.Transactions.SingleOrDefaultAsync(transaction =>
                transaction.TransactionType == transactionType &&
                transaction.Name == transactionName &&
                transaction.CategoryId == categoryId &&
                transaction.Description == description &&
                transaction.Sum == sum &&
                transaction.DateOfCreation.Date == DateTime.Now.Date &&
                transaction.DateOfEditing == null));
            Assert.Equal(transactionType, result.TransactionType);
            Assert.Equal(categoryId, result.CategoryId);
            Assert.Equal(transactionName, result.Name);
            Assert.IsType<TransactionVm>(result);
        }

        [Fact]
        public async Task CreateTransactionCommandHandler_InvalidTransactionType_InvalidEnumArgumentException()
        {
            var handler = new CreateTransactionCommandHandler(_context, _mapper);
            var invalidType = 3;
            var categoryId = 1;
            var transactionName = "New transaction";
            var description = "New description";
            var sum = 11.11M;

            await Assert.ThrowsAsync<InvalidEnumArgumentException>(async () =>
               await handler.Handle(
                   new CreateTransactionCommand
                   {
                       TransactionType = (TransactionType)invalidType,
                       CategoryId = categoryId,
                       Name = transactionName,
                       Description = description,
                       Sum = sum
                   },
                CancellationToken.None));
        }

        [Fact]
        public async Task CreateTransactionCommandHandler_InvalidCategoryId_NotFoundException()
        {
            var handler = new CreateTransactionCommandHandler(_context, _mapper);
            var transactionType = TransactionType.Income;
            var invalidCategoryId = 0;
            var transactionName = "New transaction";
            var description = "New description";
            var sum = 11.11M;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(
                   new CreateTransactionCommand
                   {
                       TransactionType = transactionType,
                       CategoryId = invalidCategoryId,
                       Name = transactionName,
                       Description = description,
                       Sum = sum
                   },
                CancellationToken.None));
        }
    }
}
