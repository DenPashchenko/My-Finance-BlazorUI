using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Transactions.Queries.GetTransactionById;
using MyFinance.WebBlazorUI.Pages.TransactionPages;

namespace MyFinance.UnitTests.PagesTests.Transactions
{
	public class TransactionDetailsPageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly IRenderedComponent<TransactionDetails> _page;
		private readonly FakeNavigationManager _navigationManager;
		private TransactionVm _transactionVm;

		public TransactionDetailsPageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_transactionVm = new()
			{
				Id = 1,
				TransactionType = Domain.Enums.TransactionType.Income,
				CategoryId = 1,
				Category = "Category1",
				Name = "Transaction1",
				Description = "Description1",
				Sum = 10.10M,
				DateOfCreation = DateTime.Now,
				DateOfEditing = null
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetTransactionByIdQuery>(), default)).ReturnsAsync(_transactionVm);

			_testContext.Services.AddSingleton(_mockMediator.Object);

			_page = _testContext.RenderComponent<TransactionDetails>(parameters => parameters.Add(p => p.Id, 1));

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchTransactionById()
		{
			var transaction = _page.Instance.TransactionVm;

			Assert.Equal(1, transaction.Id);
			Assert.Equal(Domain.Enums.TransactionType.Income, transaction.TransactionType);
			Assert.Equal("Category1", transaction.Category);
			Assert.Equal("Transaction1", transaction.Name);
			Assert.Equal("Description1", transaction.Description);
			Assert.Equal(10.10M, transaction.Sum);
			Assert.Equal(DateTime.Now.Date, transaction.DateOfCreation.Date);
			Assert.Null(transaction.DateOfEditing);
		}

		[Fact]
		public void DisplayTransactionById()
		{
			var cells = _page.FindAll("table>tbody>tr>td");
			var totalButtons = _page.FindAll("button");

			Assert.Collection(cells,
				c => Assert.Equal("Income", c.LastElementChild.TextContent),
				c => Assert.Equal("Category1", c.TextContent),
				c => Assert.Equal("Transaction1", c.TextContent),
				c => Assert.Equal("Description1", c.TextContent),
				c => Assert.Equal("10,10", c.TextContent),
				c => Assert.Equal(DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString(), c.TextContent),
				c => Assert.Equal(string.Empty, c.TextContent)
			);
			Assert.Equal(1, totalButtons.Count);
			Assert.Equal("Edit", totalButtons[0].TextContent);
		}

		[Fact]
		public void EditButton_Click_RedirectToTransactionEdit()
		{
			var editButton = _page.FindAll("button").First(b => b.TextContent == "Edit");

			editButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}transactions/edit/{_page.Instance.Id}", _navigationManager.Uri);
		}
	}
}
