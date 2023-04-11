using MediatR;
using MyFinance.Domain.Enums;
using MyFinance.Application.Transactions.Queries.GetTransactionById;

namespace MyFinance.Application.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<TransactionVm>
    {
        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Sum { get; set; }
    }
}
