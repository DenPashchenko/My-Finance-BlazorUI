using MediatR;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Interfaces;
using MyFinance.Domain;

namespace MyFinance.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
    {
        private readonly IDataDbContext _dataDbContext;

        public DeleteTransactionCommandHandler(IDataDbContext dataDbContext) => _dataDbContext = dataDbContext;

        public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var dbTransaction = await _dataDbContext.Transactions.FindAsync(request.Id);
            if (dbTransaction == null)
            {
                throw new NotFoundException(nameof(Transaction), request.Id);
            }
            _dataDbContext.Transactions.Remove(dbTransaction);
            await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
