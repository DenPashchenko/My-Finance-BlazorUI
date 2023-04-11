using MediatR;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Interfaces;
using MyFinance.Application.Properties;
using MyFinance.Domain;
using MyFinance.Domain.Enums;
using System.ComponentModel;

namespace MyFinance.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
    {
        private readonly IDataDbContext _dataDbContext;

        public UpdateTransactionCommandHandler(IDataDbContext dataDbContext) => _dataDbContext = dataDbContext;

        public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var dbTransaction = await _dataDbContext.Transactions.FindAsync(request.Id);
            if (dbTransaction == null)
            {
                throw new NotFoundException(nameof(Transaction), request.Id);
            }

            var dbCategory = await _dataDbContext.Categories.FindAsync(request.CategoryId);
            if (dbCategory == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            if (!Enum.IsDefined(typeof(TransactionType), request.TransactionType))
            {
                throw new InvalidEnumArgumentException(Resources.IrrelevantTransactionType);
            }

            dbTransaction.TransactionType = request.TransactionType;
            dbTransaction.CategoryId = request.CategoryId;
            dbTransaction.Name = request.Name;
            dbTransaction.Description = request.Description;
            dbTransaction.Sum = request.Sum;
            dbTransaction.DateOfEditing = DateTime.Now;

            await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
