using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Categories.Commands.DeleteCategory;
using MyFinance.Application.Common.Exceptions;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Categories.CommandTests
{
    public class DeleteCategoryCommandHandlerTests : TestFixtureBase
    {
        [Fact]
        public async Task DeleteCategoryCommandHandler_ValidData_Success()
        {
            var handler = new DeleteCategoryCommandHandler(_context);
            int idToDelete = 3;

            await handler.Handle(
                new DeleteCategoryCommand
                {
                    Id = idToDelete
                },
                CancellationToken.None);

            Assert.Null(await _context.Categories.SingleOrDefaultAsync(category =>
                category.Id == idToDelete));
        }

        [Fact]
        public async Task DeleteCategoryCommandHandler_InvalidId_NotFoundException()
        {
            var handler = new DeleteCategoryCommandHandler(_context);
            int invalidId = 0;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(
                   new DeleteCategoryCommand
                   {
                       Id = invalidId
                   },
                   CancellationToken.None));
        }
    }
}
