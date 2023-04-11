using Microsoft.AspNetCore.Components;
using MediatR;
using MyFinance.Application.Transactions.Commands.DeleteTransaction;
using MyFinance.Application.Transactions.Queries.GetTransactionById;

namespace MyFinance.WebBlazorUI.Pages.TransactionPages
{
	public partial class TransactionDelete
	{
		[Parameter]
		public int Id { get; set; }

		public TransactionVm TransactionVm { get; set; } = new();

		private IMediator? _mediator;
		protected override async Task OnParametersSetAsync()
		{
			_mediator ??= Mediator;
			var query = new GetTransactionByIdQuery
			{
				Id = Id
			};
			TransactionVm = await _mediator.Send(query);
		}

		private async Task DeleteTransactionAsync()
		{
			var command = new DeleteTransactionCommand
			{
				Id = Id
			};
			await Mediator.Send(command);
			NavigationManager.NavigateTo("transactions");
		}
	}
}