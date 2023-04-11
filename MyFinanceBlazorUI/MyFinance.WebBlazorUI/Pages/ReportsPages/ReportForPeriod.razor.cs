using Microsoft.AspNetCore.Components.Forms;
using MediatR;
using MyFinance.Application.Reports.Queries.ReportForPeriod;
using MyFinance.WebBlazorUI.Properties;

namespace MyFinance.WebBlazorUI.Pages.ReportsPages
{
	public partial class ReportForPeriod
	{
		public class ModelClass
		{
			public DateTime StartDate { get; set; }

			public DateTime EndDate { get; set; }
		}

		public ReportForPeriodVm ReportForPeriodVm { get; set; }
		public ModelClass Model { get; set; } = new ModelClass
		{
			StartDate = DateTime.Now.Date,
			EndDate = DateTime.Now.Date
		};
		public ValidationMessageStore? MessageStore { get; private set; }
		public EditContext? EditContext { get; private set; }

		private IMediator? _mediator;
		
		protected override void OnInitialized()
		{
			EditContext = new EditContext(Model);
			EditContext.OnValidationRequested += HandleValidationRequested;
			MessageStore = new(EditContext);
		}

		private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
		{
			MessageStore?.Clear();
			if (Model.StartDate > Model.EndDate)
			{
				MessageStore?.Add(() => Model.StartDate, Resources.StartDateError);
			}
		}

		private async Task HandleValidSubmitAsync()
		{
			_mediator ??= Mediator;
			var query = new GetReportForPeriodQuery(Model.StartDate.ToShortDateString(), Model.EndDate.ToShortDateString());
			ReportForPeriodVm = await Mediator.Send(query);
		}
	}
}