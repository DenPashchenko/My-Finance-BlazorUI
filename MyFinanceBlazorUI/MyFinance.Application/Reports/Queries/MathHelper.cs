using MyFinance.Application.Transactions.Queries.GetTransactionList;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Reports.Queries
{
    internal static class MathHelper
    {
        internal static decimal GetSum(TransactionType transactionType, List<TransactionListDto> transactions)
        {
            decimal sum = 0;
            var typedTransactions = transactions.Where(t => t.TransactionType == transactionType);
            foreach (var transaction in typedTransactions)
            {
                sum += transaction.Sum;
            }

            return sum;
        }
    }
}
