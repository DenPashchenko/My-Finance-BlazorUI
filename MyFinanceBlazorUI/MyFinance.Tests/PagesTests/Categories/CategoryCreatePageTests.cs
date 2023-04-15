using Bunit.TestDoubles;
using Bunit;
using MyFinance.WebBlazorUI.Pages.CategoryPages;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MediatR;
using Moq;
using MyFinance.WebBlazorUI.Models.CategoryDtos;

namespace MyFinance.UnitTests.PagesTests.Categories
{
	public class CategoryCreatePageTests
	{
		private readonly TestContext _testContext;
		private readonly IRenderedComponent<CategoryCreate> _page;
		private readonly FakeNavigationManager _navigationManager;
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<IMapper> _mockMapper;
		private CreateCategoryDto _createCategoryDto;

		public CategoryCreatePageTests()
        {
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_mockMapper = new Mock<IMapper>();
			_createCategoryDto = new()
			{
				Name = "Category1"
			};

			_testContext.Services.AddSingleton(_mockMediator.Object);
			_testContext.Services.AddSingleton(_mockMapper.Object);

			_page = _testContext.RenderComponent<CategoryCreate>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void SaveButtonClick_ValidInput_RedirectToCategories()
		{
			var nameInput = _page.Find("#name");
			
			nameInput.Change(_createCategoryDto.Name);
			var saveButton = _page.FindAll("button").First(b => b.TextContent == "Save");
			
			saveButton.Click();
			
			Assert.Equal($"{_navigationManager.BaseUri}categories", _navigationManager.Uri);
		}

		[Fact]
		public void SaveButtonClick_InvalidInput_NotRedirected()
		{
			var nameInput = _page.Find("#name");
			var invalidInput = "S"; // 1 char is not valid for a category name

			nameInput.Change(invalidInput);
			var saveButton = _page.FindAll("button").First(b => b.TextContent == "Save");

			saveButton.Click();

			Assert.Equal(_navigationManager.BaseUri, _navigationManager.Uri);
		}
	}

	
}
