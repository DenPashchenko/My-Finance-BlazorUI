using AutoMapper;
using MyFinance.Application.Reports.Queries.ReportForDate;
using MyFinance.Persistence;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Reports.QueryTests
{
    [Collection("QueryCollection")]
    public class GetReportForDateQueryHandlerTests
    {
        private DataDbContext _context;
        private IMapper _mapper;

        public GetReportForDateQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.context;
            _mapper = fixture._mapper;
        }

        [Fact]
        public async Task GetReportForDateQueryHandler_Yesterday_Success()
        {
            var handler = new GetReportForDateQueryHandler(_context, _mapper);
            var yesterday = DateTime.Now.AddDays(-1).Date.ToShortDateString();

            var result = await handler.Handle(new GetReportForDateQuery(yesterday), CancellationToken.None);

            Assert.Equal(1, result.Transactions.Count);
            Assert.Equal(0, result.TotalIncome);
            Assert.Equal(50.50M, result.TotalExpences);
            Assert.Equal(yesterday, result.ForDate);
            Assert.IsType<ReportForDateVm>(result);
        }

        [Fact]
        public async Task GetReportForDateQueryHandler_Today_Success()
        {
            var handler = new GetReportForDateQueryHandler(_context, _mapper);
            var today = DateTime.Now.Date.ToShortDateString();

            var result = await handler.Handle(new GetReportForDateQuery(today), CancellationToken.None);

            Assert.Equal(4, result.Transactions.Count);
            Assert.Equal(30.30M, result.TotalIncome);
            Assert.Equal(70.70M, result.TotalExpences);
            Assert.Equal(today, result.ForDate);
        }

        [Fact]
        public async Task GetReportForDateQueryHandler_Tommorow_ValidationException()
        {
            var handler = new GetReportForDateQueryHandler(_context, _mapper);
            var tommorow = DateTime.Now.AddDays(1).Date.ToShortDateString();

            await Assert.ThrowsAsync<ValidationException>(async () =>
               await handler.Handle(new GetReportForDateQuery(tommorow), CancellationToken.None));
        }

        [Fact]
        public async Task GetReportForDateQueryHandler_InvalidDateValue_ValidationException()
        {
            var handler = new GetReportForDateQueryHandler(_context, _mapper);
            var invalidDateValue = "invalid date";

            await Assert.ThrowsAsync<ValidationException>(async () =>
               await handler.Handle(new GetReportForDateQuery(invalidDateValue), CancellationToken.None));
        }
    }
}
