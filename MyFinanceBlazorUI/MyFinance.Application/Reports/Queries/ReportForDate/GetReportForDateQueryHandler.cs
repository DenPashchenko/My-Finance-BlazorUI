using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Interfaces;
using MyFinance.Application.Transactions.Queries.GetTransactionList;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Reports.Queries.ReportForDate
{
    public class GetReportForDateQueryHandler : IRequestHandler<GetReportForDateQuery, ReportForDateVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public GetReportForDateQueryHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<ReportForDateVm> Handle(GetReportForDateQuery request, CancellationToken cancellationToken)
        {
            var transactionsForDateQuery = await _dataDbContext.Transactions
                .Where(t => t.DateOfCreation.Date == request.Date.Date)
                .ProjectTo<TransactionListDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var incomeForDate = MathHelper.GetSum(TransactionType.Income, transactionsForDateQuery);
            var expencesForDate = MathHelper.GetSum(TransactionType.Expenses, transactionsForDateQuery);

            var reportForDateVm = new ReportForDateVm
            {
                Transactions = transactionsForDateQuery,
                TotalIncome = incomeForDate,
                TotalExpences = expencesForDate,
                ForDate = request.Date.ToShortDateString()
            };

            return reportForDateVm;
        }
    }
}
