using MediatR;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommand : IRequest
    {
        public int Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Sum { get; set; }
    }
}
