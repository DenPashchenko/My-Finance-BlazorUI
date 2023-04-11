using Microsoft.AspNetCore.Components;
using MediatR;
using MyFinance.Application.Categories.Queries.GetCategoryById;

namespace MyFinance.WebBlazorUI.Pages.CategoryPages
{
	public partial class CategoryDetails
	{
		[Parameter]
		public int Id { get; set; }

		public CategoryVm CategoryVm { get; set; } = new();
		private IMediator _mediator;
		protected override async Task OnParametersSetAsync()
		{
			_mediator ??= Mediator;
			var query = new GetCategoryByIdQuery
			{
				Id = Id
			};
			CategoryVm = await _mediator.Send(query);
		}

		private void EditCategory(int id)
		{
			NavigationManager.NavigateTo($"/categories/edit/{id}");
		}
	}
}