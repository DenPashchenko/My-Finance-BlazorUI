using AutoMapper;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Transactions.Queries.GetTransactionById;
using MyFinance.Persistence;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Transactions.QueryTests
{
    [Collection("QueryCollection")]
    public class GetTransactionByIdQueryHandlerTests
    {
        private DataDbContext _context;
        private IMapper _mapper;

        public GetTransactionByIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.context;
            _mapper = fixture._mapper;
        }

        [Fact]
        public async Task GetTransactionByIdQueryHandler_ValidId_Success()
        {
            var handler = new GetTransactionByIdQueryHandler(_context, _mapper);
            int id = 1;

            var result = await handler.Handle(
                new GetTransactionByIdQuery
                {
                    Id = id
                },
                CancellationToken.None);

            Assert.IsType<TransactionVm>(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(Domain.Enums.TransactionType.Income, result.TransactionType);
            Assert.Equal(1, result.CategoryId);
            Assert.Equal("Category1", result.Category);
            Assert.Equal("Transaction1", result.Name);
            Assert.Equal("Description1", result.Description);
            Assert.Equal(10.10M, result.Sum);

        }

        [Fact]
        public async Task GetTransactionByIdQueryHandler_InvalidId_NotFoundException()
        {
            var handler = new GetTransactionByIdQueryHandler(_context, _mapper);
            int invalidId = 0;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(
                   new GetTransactionByIdQuery
                   {
                       Id = invalidId
                   },
                   CancellationToken.None));
        }
    }
}
