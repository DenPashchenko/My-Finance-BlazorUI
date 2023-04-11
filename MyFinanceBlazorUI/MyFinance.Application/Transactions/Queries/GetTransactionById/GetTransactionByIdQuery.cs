using MediatR;

namespace MyFinance.Application.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : IRequest<TransactionVm>
    {
        public int Id { get; set; }
    }
}
