using AutoMapper;
using MyFinance.Application.Transactions.Queries.GetTransactionList;
using MyFinance.Persistence;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Transactions.QueryTests
{
    [Collection("QueryCollection")]
    public class GetTransactionListQueryHandlerTest
    {
        private DataDbContext _context;
        private IMapper _mapper;

        public GetTransactionListQueryHandlerTest(QueryTestFixture fixture)
        {
            _context = fixture.context;
            _mapper = fixture._mapper;
        }

        [Fact]
        public async Task GetTransactionListQueryHandler_Success()
        {
            var handler = new GetTransactionListQueryHandler(_context, _mapper);

            var result = await handler.Handle(new GetTransactionListQuery(), CancellationToken.None);

            Assert.Equal(_context.Transactions.Count(), result.Transactions.Count());
            Assert.IsType<TransactionListVm>(result);
        }
    }
}
