using AutoMapper;
using MyFinance.Application.Categories.Queries.GetCategoryList;
using MyFinance.Persistence;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Categories.QueryTests
{
    [Collection("QueryCollection")]
    public class GetCategoryListQueryHandlerTest
    {
        private DataDbContext _context;
        private IMapper _mapper;

        public GetCategoryListQueryHandlerTest(QueryTestFixture fixture)
        {
            _context = fixture.context;
            _mapper = fixture._mapper;
        }

        [Fact]
        public async Task GetCategoryListQueryHandler_Success()
        {
            var handler = new GetCategoryListQueryHandler(_context, _mapper);

            var result = await handler.Handle(new GetCategoryListQuery(), CancellationToken.None);

            Assert.Equal(_context.Categories.Count(), result.Categories.Count());
            Assert.IsType<CategoryListVm>(result);
        }
    }
}
