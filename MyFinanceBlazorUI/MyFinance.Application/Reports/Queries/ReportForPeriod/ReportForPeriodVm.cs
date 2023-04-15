using MyFinance.Application.Transactions.Queries.GetTransactionList;

namespace MyFinance.Application.Reports.Queries.ReportForPeriod
{
    public class ReportForPeriodVm
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpences { get; set; }

        public string ForPeriod { get; set; }

        public IList<TransactionListDto> Transactions { get; set; }
    }
}
