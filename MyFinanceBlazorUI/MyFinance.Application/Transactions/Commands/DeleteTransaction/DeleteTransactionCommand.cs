using MediatR;

namespace MyFinance.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommand : IRequest
    {
        public int Id { get; set; }
    }
}
