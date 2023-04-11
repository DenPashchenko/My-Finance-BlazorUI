using Microsoft.AspNetCore.Components;
using MediatR;
using MyFinance.Application.Transactions.Queries.GetTransactionList;

namespace MyFinance.WebBlazorUI.Pages.TransactionPages
{
	public partial class Transactions
	{
		public TransactionListVm TransactionListVm { get; set; }

		private IList<TransactionListDto>? transactions;
		private IMediator? _mediator;
		protected override async Task OnInitializedAsync()
		{
			_mediator ??= Mediator;
			TransactionListVm = new();
			transactions = new List<TransactionListDto>();
			var query = new GetTransactionListQuery();

			TransactionListVm = await _mediator.Send(query);
			transactions = TransactionListVm.Transactions;
		}

		private void ShowTransaction(int id)
		{
			NavigationManager.NavigateTo($"transactions/details/{id}");
		}

		private void EditTransaction(int id)
		{
			NavigationManager.NavigateTo($"transactions/edit/{id}");
		}

		private void DeleteTransaction(int id)
		{
			NavigationManager.NavigateTo($"transactions/delete/{id}");
		}

		private void CreateTransaction()
		{
			NavigationManager.NavigateTo($"transactions/create");
		}
	}
}