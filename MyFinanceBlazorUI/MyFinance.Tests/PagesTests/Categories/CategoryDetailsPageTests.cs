using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using MyFinance.Application.Categories.Queries.GetCategoryById;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.WebBlazorUI.Pages.CategoryPages;

namespace MyFinance.UnitTests.PagesTests.Categories
{
	public class CategoryDetailsPageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly IRenderedComponent<CategoryDetails> _page;
		private readonly FakeNavigationManager _navigationManager;
		private CategoryVm _categoryVm;

		public CategoryDetailsPageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_categoryVm = new()
			{
				Id = 1,
				Name = "Category1",
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), default)).ReturnsAsync(_categoryVm);

			_testContext.Services.AddSingleton(_mockMediator.Object);

			_page = _testContext.RenderComponent<CategoryDetails>(parameters => parameters.Add(p => p.Id, 1));

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchCategoryById()
		{
			var category = _page.Instance.CategoryVm;

			Assert.Equal(1, category.Id);
			Assert.Equal("Category1", category.Name);
		}

		[Fact]
		public void DisplayCategoryById()
		{
			var cells = _page.FindAll("table>tbody>tr>td");
			var totalButtons = _page.FindAll("button");

			Assert.Collection(cells,
				c => Assert.Equal("Category1", c.TextContent)
			);
			Assert.Equal(1, totalButtons.Count);
			Assert.Equal("Edit", totalButtons[0].TextContent);
		}

		[Fact]
		public void EditButton_Click_RedirectToCategoryEdit()
		{
			var editButton = _page.FindAll("button").First(b => b.TextContent == "Edit");

			editButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}categories/edit/{_page.Instance.Id}", _navigationManager.Uri);
		}
	}
}
