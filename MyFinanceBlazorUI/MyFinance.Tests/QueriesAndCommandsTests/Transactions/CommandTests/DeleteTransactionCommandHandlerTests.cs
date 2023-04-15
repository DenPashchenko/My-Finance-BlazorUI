using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Transactions.Commands.DeleteTransaction;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Transactions.CommandTests
{
    public class DeleteTransactionCommandHandlerTests : TestFixtureBase
    {
        [Fact]
        public async Task DeleteTransactionCommandHandler_ValidData_Success()
        {
            var handler = new DeleteTransactionCommandHandler(_context);
            int idToDelete = 3;

            await handler.Handle(
                new DeleteTransactionCommand
                {
                    Id = idToDelete
                },
                CancellationToken.None);

            Assert.Null(await _context.Transactions.SingleOrDefaultAsync(transaction =>
                transaction.Id == idToDelete));
        }

        [Fact]
        public async Task DeleteTransactionCommandHandler_InvalidId_NotFoundException()
        {
            var handler = new DeleteTransactionCommandHandler(_context);
            int invalidId = 0;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(
                   new DeleteTransactionCommand
                   {
                       Id = invalidId
                   },
                   CancellationToken.None));
        }
    }
}
