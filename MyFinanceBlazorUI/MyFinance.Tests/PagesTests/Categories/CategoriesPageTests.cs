using Bunit;
using Bunit.TestDoubles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyFinance.Application.Categories.Queries.GetCategoryList;

namespace MyFinance.UnitTests.PagesTests.Categories
{
	public class CategoriesPageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly IRenderedComponent<WebBlazorUI.Pages.CategoryPages.Categories> _page;
		private readonly FakeNavigationManager _navigationManager;
		private CategoryListVm _categoryListVm;

		public CategoriesPageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_categoryListVm = new();

			var categories = new List<CategoryListDTO>
			{
				new(){ Id = 1, Name = "Category1"},
				new(){ Id = 2, Name = "Category2"},
			};
			_categoryListVm.Categories = categories;

			_mockMediator.Setup(m => m.Send(It.IsAny<GetCategoryListQuery>(), default)).ReturnsAsync(_categoryListVm);

			_testContext.Services.AddSingleton(_mockMediator.Object);

			_page = _testContext.RenderComponent<WebBlazorUI.Pages.CategoryPages.Categories>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchCategories()
		{
			var categories = _page.Instance.CategoryListVm.Categories;

			Assert.Collection(categories,
				c =>
				{
					Assert.Equal(1, c.Id);
					Assert.Equal("Category1", c.Name);
				},
				c =>
				{
					Assert.Equal(2, c.Id);
					Assert.Equal("Category2", c.Name);
				});
		}

		[Fact]
		public void DisplayCategories()
		{
			var cells = _page.FindAll("table>tbody>tr>td");
			var totalButtons = _page.FindAll("button");

			Assert.Collection(cells,
				c => Assert.Equal("Category1", c.TextContent),
				c => Assert.Equal(3, c.ChildElementCount), // 3 buttons

				c => Assert.Equal("Category2", c.TextContent),
				c => Assert.Equal(3, c.ChildElementCount) // 3 buttons
			);
			Assert.Equal(7, totalButtons.Count);
			Assert.Equal("Create a new category", totalButtons[0].TextContent);
		}

		[Fact]
		public void CreateButton_Click_RedirectToCategoryCreate()
		{
			var createButton = _page.FindAll("button").First(b => b.TextContent == "Create a new category");

			createButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}categories/create", _navigationManager.Uri);
		}
	}
}
