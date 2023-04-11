using AutoMapper;
using MediatR;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Interfaces;
using MyFinance.Application.Properties;
using MyFinance.Application.Transactions.Queries.GetTransactionById;
using MyFinance.Domain;
using MyFinance.Domain.Enums;
using System.ComponentModel;

namespace MyFinance.Application.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<TransactionVm> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var dbCategory = await _dataDbContext.Categories.FindAsync(request.CategoryId);
            if (dbCategory == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }
            if (!Enum.IsDefined(typeof(TransactionType), request.TransactionType))
            {
                throw new InvalidEnumArgumentException(Resources.IrrelevantTransactionType);
            }
            var transaction = new Transaction
            {
                TransactionType = request.TransactionType,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                Sum = request.Sum,
                DateOfCreation = DateTime.Now,
                DateOfEditing = null
            };

            await _dataDbContext.Transactions.AddAsync(transaction, cancellationToken);
            await _dataDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransactionVm>(transaction);
        }
    }
}
