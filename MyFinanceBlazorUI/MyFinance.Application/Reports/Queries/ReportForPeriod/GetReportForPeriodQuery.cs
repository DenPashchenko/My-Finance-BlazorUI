using MediatR;
using MyFinance.Application.Properties;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Application.Reports.Queries.ReportForPeriod
{
    public class GetReportForPeriodQuery : IRequest<ReportForPeriodVm>
    {
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public GetReportForPeriodQuery(string requestStartDate, string requestEndDate)
        {
            if (DateTime.TryParse(requestStartDate, out var startDate)
                && DateTime.TryParse(requestEndDate, out var endDate)
                && startDate <= endDate
                && endDate <= DateTime.Now)
            {
                StartDate = startDate;
                EndDate = endDate;
            }
            else
            {
                throw new ValidationException(Resources.InvalidDates);
            }
        }
    }
}
