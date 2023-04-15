using AutoMapper;
using MyFinance.Application.Reports.Queries.ReportForPeriod;
using MyFinance.Persistence;
using MyFinance.UnitTests.QueriesAndCommandsTests.Common;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Reports.QueryTests
{
    [Collection("QueryCollection")]
    public class GetReportForPeriodQueryHandlerTests
    {
        private DataDbContext _context;
        private IMapper _mapper;

        public GetReportForPeriodQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.context;
            _mapper = fixture._mapper;
        }

        [Fact]
        public async Task GetReportForPeriodQueryHandler_ValidDates_Success()
        {
            var handler = new GetReportForPeriodQueryHandler(_context, _mapper);
            var yesterday = DateTime.Now.AddDays(-1).Date.ToShortDateString();
            var today = DateTime.Now.Date.ToShortDateString();
            var period = yesterday + " - " + today;

            var result = await handler.Handle(new GetReportForPeriodQuery(yesterday, today), CancellationToken.None);

            Assert.Equal(5, result.Transactions.Count);
            Assert.Equal(30.30M, result.TotalIncome);
            Assert.Equal(121.20M, result.TotalExpences);
            Assert.Equal(period, result.ForPeriod);
            Assert.IsType<ReportForPeriodVm>(result);
        }

        [Fact]
        public async Task GetReportForPeriodQueryHandler_TommorowEndDate_ValidationException()
        {
            var handler = new GetReportForPeriodQueryHandler(_context, _mapper);
            var tommorow = DateTime.Now.AddDays(1).Date.ToShortDateString();
            var today = DateTime.Now.Date.ToShortDateString();

            await Assert.ThrowsAsync<ValidationException>(async () =>
               await handler.Handle(new GetReportForPeriodQuery(today, tommorow), CancellationToken.None));
        }

        [Fact]
        public async Task GetReportForPeriodQueryHandler_StartDateBiggerThanEndDate_ValidationException()
        {
            var handler = new GetReportForPeriodQueryHandler(_context, _mapper);
            var yesterday = DateTime.Now.AddDays(-1).Date.ToShortDateString();
            var today = DateTime.Now.Date.ToShortDateString();

            await Assert.ThrowsAsync<ValidationException>(async () =>
               await handler.Handle(new GetReportForPeriodQuery(today, yesterday), CancellationToken.None));
        }

        [Fact]
        public async Task GetReportForPeriodQueryHandler_InvalidStartDateValue_ValidationException()
        {
            var handler = new GetReportForPeriodQueryHandler(_context, _mapper);
            var invalidDateValue = "invalid date";
            var today = DateTime.Now.Date.ToShortDateString();

            await Assert.ThrowsAsync<ValidationException>(async () =>
               await handler.Handle(new GetReportForPeriodQuery(invalidDateValue, today), CancellationToken.None));
        }

        [Fact]
        public async Task GetReportForPeriodQueryHandler_InvalidEndDateValue_ValidationException()
        {
            var handler = new GetReportForPeriodQueryHandler(_context, _mapper);
            var invalidDateValue = "invalid date";
            var today = DateTime.Now.Date.ToShortDateString();

            await Assert.ThrowsAsync<ValidationException>(async () =>
               await handler.Handle(new GetReportForPeriodQuery(today, invalidDateValue), CancellationToken.None));
        }
    }
}
