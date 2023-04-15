using Microsoft.AspNetCore.Components;
using MediatR;
using AutoMapper;
using MyFinance.WebBlazorUI.Models.CategoryDtos;
using MyFinance.Application.Categories.Commands.CreateCategory;

namespace MyFinance.WebBlazorUI.Pages.CategoryPages
{
	public partial class CategoryCreate
	{
		public CreateCategoryDto CreateCategoryDto { get; set; } = new();

		private IMediator? _mediator;
		private IMapper _mapper;

		private async Task HandleValidSubmitAsync()
		{
			_mediator ??= Mediator;
			_mapper = Mapper;

			var command = _mapper.Map<CreateCategoryCommand>(CreateCategoryDto);
			_ = await _mediator.Send(command);
			NavigationManager.NavigateTo("categories");
		}
	}
}