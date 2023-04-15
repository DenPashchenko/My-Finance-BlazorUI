using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Domain;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public GetTransactionByIdQueryHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<TransactionVm> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _dataDbContext.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), request.Id);
            }

            return _mapper.Map<TransactionVm>(transaction);
        }
    }
}
