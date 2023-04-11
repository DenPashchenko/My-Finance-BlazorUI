using Microsoft.AspNetCore.Components;
using MediatR;
using MyFinance.Application.Categories.Queries.GetCategoryList;

namespace MyFinance.WebBlazorUI.Pages.CategoryPages
{
	public partial class Categories
	{
		public CategoryListVm CategoryListVm { get; set; }

		private IList<CategoryListDTO> categories = new List<CategoryListDTO>();
		private IMediator? _mediator;

		protected override async Task OnInitializedAsync()
		{
			_mediator ??= Mediator;
			CategoryListVm = new();

			var query = new GetCategoryListQuery();
			CategoryListVm = await _mediator.Send(query);
			categories = CategoryListVm.Categories;
		}

		private void ShowCategory(int id)
		{
			NavigationManager.NavigateTo($"categories/details/{id}");
		}

		private void EditCategory(int id)
		{
			NavigationManager.NavigateTo($"categories/edit/{id}");
		}

		private void DeleteCategory(int id)
		{
			NavigationManager.NavigateTo($"categories/delete/{id}");
		}

		private void CreateCategory()
		{
			NavigationManager.NavigateTo($"categories/create");
		}
	}
}