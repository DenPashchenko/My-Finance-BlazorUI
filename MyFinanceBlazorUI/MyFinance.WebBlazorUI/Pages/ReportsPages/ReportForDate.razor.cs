using MediatR;
using MyFinance.Application.Reports.Queries.ReportForDate;

namespace MyFinance.WebBlazorUI.Pages.ReportsPages
{
	public partial class ReportForDate
	{
		public DateTime Date { get; set; } = DateTime.Now.Date;
		public ReportForDateVm? ReportForDateVm { get; set; }

		private IMediator? _mediator;

		private async Task HandleValidSubmitAsync()
		{
			_mediator ??= Mediator;

			var query = new GetReportForDateQuery(Date.ToShortDateString());
			ReportForDateVm = await _mediator.Send(query);
		}
	}
}