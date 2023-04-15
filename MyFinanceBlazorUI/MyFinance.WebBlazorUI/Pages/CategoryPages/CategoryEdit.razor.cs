using Microsoft.AspNetCore.Components;
using MediatR;
using AutoMapper;
using MyFinance.Application.Categories.Commands.UpdateCategory;
using MyFinance.Application.Categories.Queries.GetCategoryById;
using MyFinance.WebBlazorUI.Models.CategoryDtos;

namespace MyFinance.WebBlazorUI.Pages.CategoryPages
{
	public partial class CategoryEdit
	{
		[Parameter]
		public int Id { get; set; }
		public CategoryVm CategoryVm { get; set; } = new();

		private UpdateCategoryDto updateCategoryDto = new();
		private IMediator _mediator;
		private IMapper _mapper;

		protected override async Task OnParametersSetAsync()
		{
			_mediator ??= Mediator;
			var query = new GetCategoryByIdQuery
			{
				Id = Id
			};
			CategoryVm = await _mediator.Send(query);
			updateCategoryDto.Name = CategoryVm.Name;
		}

		private async Task HandleValidSubmitAsync()
		{
			_mediator ??= Mediator;
			_mapper = Mapper;

			var command = _mapper.Map<UpdateCategoryCommand>(updateCategoryDto);
			command.Id = Id;
			await Mediator.Send(command);

			NavigationManager.NavigateTo("categories");
		}
	}
}