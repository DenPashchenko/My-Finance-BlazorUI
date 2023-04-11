using Microsoft.AspNetCore.Components;
using MediatR;
using AutoMapper;
using MyFinance.Application.Categories.Queries.GetCategoryList;
using MyFinance.Application.Transactions.Commands.UpdateTransaction;
using MyFinance.Application.Transactions.Queries.GetTransactionById;
using MyFinance.WebBlazorUI.Models.TransactionDtos;

namespace MyFinance.WebBlazorUI.Pages.TransactionPages
{
	public partial class TransactionEdit
	{
		[Parameter]
		public int Id { get; set; }
		public TransactionVm TransactionVm { get; set; } = new();

		private UpdateTransactionDto updateTransactionDto = new();
		private CategoryListVm? categoryListVm;
		private IList<CategoryListDTO>? categories;
		private IMediator? _mediator;
		private IMapper? _mapper;
		protected override async Task OnInitializedAsync()
		{
			_mediator ??= Mediator;
			categoryListVm = new();
			categories = new List<CategoryListDTO>();
			var query = new GetCategoryListQuery();
			categoryListVm = await _mediator.Send(query);
			categories = categoryListVm.Categories;
		}

		protected override async Task OnParametersSetAsync()
		{
			_mediator ??= Mediator;
			var query = new GetTransactionByIdQuery
			{
				Id = Id
			};
			TransactionVm = await _mediator.Send(query);
			updateTransactionDto.TransactionType = TransactionVm.TransactionType;
			updateTransactionDto.CategoryId = TransactionVm.CategoryId;
			updateTransactionDto.Name = TransactionVm.Name;
			updateTransactionDto.Description = TransactionVm.Description;
			updateTransactionDto.Sum = TransactionVm.Sum;
		}

		private async Task HandleValidSubmitAsync()
		{
			_mediator ??= Mediator;
			_mapper = Mapper;
			var command = _mapper.Map<UpdateTransactionCommand>(updateTransactionDto);
			command.Id = Id;
			await Mediator.Send(command);
			NavigationManager.NavigateTo("transactions");
		}
	}
}