using MediatR;
using MyFinance.Application.Properties;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Application.Reports.Queries.ReportForDate
{
    public class GetReportForDateQuery : IRequest<ReportForDateVm>
    {
        public DateTime Date { get; private set; }

        public GetReportForDateQuery(string request)
        {
            if (DateTime.TryParse(request, out var date) && date <= DateTime.Now)
            {
                Date = date;
            }
            else
            {
                throw new ValidationException(Resources.InvalidDate);
            }
        }
    }
}
