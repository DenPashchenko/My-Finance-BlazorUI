using Microsoft.AspNetCore.Components;
using MediatR;
using AutoMapper;
using MyFinance.Application.Categories.Queries.GetCategoryList;
using MyFinance.Application.Transactions.Commands.CreateTransaction;
using MyFinance.WebBlazorUI.Models.TransactionDtos;
using MyFinance.WebBlazorUI.Properties;

namespace MyFinance.WebBlazorUI.Pages.TransactionPages
{
	public partial class TransactionCreate
	{
		public CreateTransactionDto? CreateTransactionDto { get; set; }

		private CategoryListVm? categoryListVm;
		private IList<CategoryListDTO>? categories;
		private IMediator? _mediator;
		private IMapper? _mapper;

		protected override async Task OnInitializedAsync()
		{
			_mediator ??= Mediator;
			categoryListVm = new();
			CreateTransactionDto = new();

			categories = new List<CategoryListDTO>();
			var query = new GetCategoryListQuery();
			categoryListVm = await _mediator.Send(query);
			if (categoryListVm.Categories.Count == 0)
			{
				throw new InvalidOperationException(Resources.DefineCategoryFirst);
			}

			categories = categoryListVm.Categories;
			CreateTransactionDto.CategoryId = categories[0].Id;
		}

		private async Task HandleValidSubmitAsync()
		{
			_mediator ??= Mediator;
			_mapper = Mapper;

			var command = _mapper.Map<CreateTransactionCommand>(CreateTransactionDto);
			_ = await Mediator.Send(command);

			NavigationManager.NavigateTo("transactions");
		}
	}
}