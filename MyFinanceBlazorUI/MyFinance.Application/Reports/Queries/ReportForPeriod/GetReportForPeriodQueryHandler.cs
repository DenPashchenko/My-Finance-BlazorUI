using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Interfaces;
using MyFinance.Application.Transactions.Queries.GetTransactionList;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Reports.Queries.ReportForPeriod
{
    public class GetReportForPeriodQueryHandler : IRequestHandler<GetReportForPeriodQuery, ReportForPeriodVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public GetReportForPeriodQueryHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<ReportForPeriodVm> Handle(GetReportForPeriodQuery request, CancellationToken cancellationToken)
        {
            var transactionsForPeriodQuery = await _dataDbContext.Transactions
                .Where(t => t.DateOfCreation.Date >= request.StartDate.Date && t.DateOfCreation.Date <= request.EndDate.Date)
                .ProjectTo<TransactionListDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var incomeForPeriod = MathHelper.GetSum(TransactionType.Income, transactionsForPeriodQuery);
            var expencesForPeriod = MathHelper.GetSum(TransactionType.Expenses, transactionsForPeriodQuery);

            var reportForPeriodVm = new ReportForPeriodVm
            {
                Transactions = transactionsForPeriodQuery,
                TotalIncome = incomeForPeriod,
                TotalExpences = expencesForPeriod,
                ForPeriod = request.StartDate.ToShortDateString() + " - " + request.EndDate.ToShortDateString()
            };

            return reportForPeriodVm;
        }
    }
}
