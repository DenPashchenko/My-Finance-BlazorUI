using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Transactions.Queries.GetTransactionList
{
    public class GetTransactionListQueryHandler : IRequestHandler<GetTransactionListQuery, TransactionListVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public GetTransactionListQueryHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<TransactionListVm> Handle(GetTransactionListQuery request, CancellationToken cancellationToken)
        {
            var transactionsQuery = await _dataDbContext.Transactions
                .ProjectTo<TransactionListDto>(_mapper.ConfigurationProvider)
                .OrderBy(comparer => comparer.DateOfCreation)
                .ToListAsync(cancellationToken);

            return new TransactionListVm { Transactions = transactionsQuery };
        }
    }
}
