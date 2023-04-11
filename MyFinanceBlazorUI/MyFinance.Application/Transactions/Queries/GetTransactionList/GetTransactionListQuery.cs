using MediatR;
using MyFinance.Domain.Enums;
using MyFinance.Domain;

namespace MyFinance.Application.Transactions.Queries.GetTransactionList
{
    public class GetTransactionListQuery : IRequest<TransactionListVm>
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Sum { get; set; }

        public DateTime DateOfCreation { get; set; }

        public DateTime? DateOfEditing { get; set; }
    }
}
