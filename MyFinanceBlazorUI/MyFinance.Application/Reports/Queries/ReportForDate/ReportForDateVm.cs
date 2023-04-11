using MyFinance.Application.Transactions.Queries.GetTransactionList;

namespace MyFinance.Application.Reports.Queries.ReportForDate
{
    public class ReportForDateVm
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpences { get; set; }

        public string ForDate { get; set; }

        public IList<TransactionListDto> Transactions { get; set; }
    }
}
