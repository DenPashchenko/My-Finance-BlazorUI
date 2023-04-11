using Bunit.TestDoubles;
using Bunit;
using Index = MyFinance.WebBlazorUI.Pages.Index;
using Microsoft.Extensions.DependencyInjection;

namespace MyFinance.UnitTests.PagesTests
{
	public class IndexPageTests
	{
		private readonly TestContext _testContext;
		private readonly IRenderedComponent<Index> _page;
		private readonly FakeNavigationManager _navigationManager;

		public IndexPageTests()
		{
			_testContext = new TestContext();

			_page = _testContext.RenderComponent<Index>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void DisplayCardsWithButtons()
		{
			var totalCards = _page.FindAll("div").Where(d => d.ClassList.Contains("card"));
			var totalButtons = _page.FindAll("div>a").Where(a => a.ClassList.Contains("btn"));
			var cardTitles = _page.FindAll("div>h5").Where(d => d.ClassName == "card-title");

			Assert.Equal(4, totalCards.Count());
			Assert.Equal(4, totalButtons.Count());
			Assert.Collection(totalButtons,
				tb =>
				{
					Assert.Equal("Start", tb.TextContent);
					Assert.Equal("categories", tb.GetAttribute("href"));
				},
				tb =>
				{
					Assert.Equal("Start", tb.TextContent);
					Assert.Equal("transactions", tb.GetAttribute("href"));
				},
				tb =>
				{
					Assert.Equal("Start", tb.TextContent);
					Assert.Equal("period_report", tb.GetAttribute("href"));
				},
				tb =>
				{
					Assert.Equal("Start", tb.TextContent);
					Assert.Equal("date_report", tb.GetAttribute("href"));
				}
			);
			Assert.Collection(cardTitles,
				ct => Assert.Equal("Categories", ct.TextContent),
				ct => Assert.Equal("Transactions", ct.TextContent),
				ct => Assert.Equal("Reports", ct.TextContent),
				ct => Assert.Equal("Reports", ct.TextContent)
			);
		}
	}
}
