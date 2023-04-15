using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Categories.Commands.CreateCategory;
using MyFinance.Application.Categories.Queries.GetCategoryById;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Categories.CommandTests
{
    public class CreateCategoryCommandHandlerTests : TestFixtureBase
    {
        [Fact]
        public async Task CreateCategoryCommandHandler_ValidData_Success()
        {
            var handler = new CreateCategoryCommandHandler(_context, _mapper);
            var categoryName = "New name";

            var result = await handler.Handle(
                new CreateCategoryCommand
                {
                    Name = categoryName,
                },
                CancellationToken.None);

            Assert.NotNull(await _context.Categories.SingleOrDefaultAsync(category => category.Name == categoryName));
            Assert.Equal(categoryName, result.Name);
            Assert.IsType<CategoryVm>(result);
        }
    }
}
