using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using MyFinance.Application.Categories.Queries.GetCategoryById;
using MyFinance.WebBlazorUI.Pages.CategoryPages;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MyFinance.WebBlazorUI.Models.CategoryDtos;
using MyFinance.Application.Categories.Commands.UpdateCategory;

namespace MyFinance.UnitTests.PagesTests.Categories
{
	public class CategoryEditPageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<IMapper> _mockMapper;
		private readonly IRenderedComponent<CategoryEdit> _page;
		private readonly FakeNavigationManager _navigationManager;
		private CategoryVm _categoryVm;
		private UpdateCategoryDto _updateCategoryDto;

		public CategoryEditPageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_mockMapper = new Mock<IMapper>();
			_categoryVm = new()
			{
				Id = 1,
				Name = "Category1",
			};
			_updateCategoryDto = new()
			{
				Name = "Category2"
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), default)).ReturnsAsync(_categoryVm);
			_mockMediator.Setup(m => m.Send(It.IsAny<UpdateCategoryCommand>(), default)).Returns(Task.CompletedTask);

			_mockMapper.Setup(m => m.Map<UpdateCategoryCommand>(It.IsAny<UpdateCategoryDto>())).Returns(new UpdateCategoryCommand());

			_testContext.Services.AddSingleton(_mockMediator.Object);
			_testContext.Services.AddSingleton(_mockMapper.Object);

			_page = _testContext.RenderComponent<CategoryEdit>(parameters => parameters.Add(p => p.Id, 1));

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchCategoryByIdBeforeEditing()
		{
			var category = _page.Instance.CategoryVm;

			Assert.Equal(1, category.Id);
			Assert.Equal("Category1", category.Name);
		}

		[Fact]
		public void DisplayCategoryByIdBeforeEditing()
		{
			var nameInput = _page.Find("#name");
			var inputValue = nameInput.GetAttribute("value");
			var totalButtons = _page.FindAll("button");

			Assert.Equal("Category1", inputValue);
			Assert.Equal(2, totalButtons.Count);
			Assert.Equal("Update", totalButtons[0].TextContent);
			Assert.Equal("Reset", totalButtons[1].TextContent);
		}

		[Fact]
		public void UpdateButtonClick_ValidInput_RedirectToCategories()
		{
			var nameInput = _page.Find("#name");

			nameInput.Change(_updateCategoryDto.Name);
			var updateButton = _page.FindAll("button").First(b => b.TextContent == "Update");

			updateButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}categories", _navigationManager.Uri);
		}

		[Fact]
		public void UpdateButtonClick_InvalidInput_NotRedirected()
		{
			var nameInput = _page.Find("#name");
			var invalidInput = "S"; // 1 char is not valid for a category name

			nameInput.Change(invalidInput);
			var updateButton = _page.FindAll("button").First(b => b.TextContent == "Update");

			updateButton.Click();

			Assert.Equal(_navigationManager.BaseUri, _navigationManager.Uri);
		}
	}
}
