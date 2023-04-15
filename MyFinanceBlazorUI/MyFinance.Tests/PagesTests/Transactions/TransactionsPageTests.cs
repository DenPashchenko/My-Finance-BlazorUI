using Bunit;
using Bunit.TestDoubles;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyFinance.Application.Transactions.Queries.GetTransactionList;
using MyFinance.WebBlazorUI.Pages;
using System.Globalization;

namespace MyFinance.UnitTests.PagesTests.Transactions
{
	public class TransactionsPageTests
	{
		public static readonly string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly IRenderedComponent<WebBlazorUI.Pages.TransactionPages.Transactions> _page;
		private readonly FakeNavigationManager _navigationManager;
		private TransactionListVm _transactionListVm;

		public TransactionsPageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_transactionListVm = new();

			var transactions = new List<TransactionListDto>
			{
				new()
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
				},
				new()
				{
					Id = 2,
					TransactionType = Domain.Enums.TransactionType.Expenses,
					CategoryId = 2,
					Category = "Category2",
					Name = "Transaction2",
					Description = "Description2",
					Sum = 20.20M,
					DateOfCreation = DateTime.Now,
					DateOfEditing = null
				}
			};

			_transactionListVm.Transactions = transactions;

			_mockMediator.Setup(m => m.Send(It.IsAny<GetTransactionListQuery>(), default)).ReturnsAsync(_transactionListVm);

			_testContext.Services.AddSingleton(_mockMediator.Object);

			_page = _testContext.RenderComponent<WebBlazorUI.Pages.TransactionPages.Transactions>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchTransactions()
		{
			var transactions = _page.Instance.TransactionListVm.Transactions;

			Assert.Collection(transactions,
				t =>
				{
					Assert.Equal(1, t.Id);
					Assert.Equal(Domain.Enums.TransactionType.Income, t.TransactionType);
					Assert.Equal("Category1", t.Category);
					Assert.Equal("Transaction1", t.Name);
					Assert.Equal("Description1", t.Description);
					Assert.Equal(10.10M, t.Sum);
					Assert.Equal(DateTime.Now.Date, t.DateOfCreation.Date);
					Assert.Null(t.DateOfEditing);
				},
				t =>
				{
					Assert.Equal(2, t.Id);
					Assert.Equal(Domain.Enums.TransactionType.Expenses, t.TransactionType);
					Assert.Equal("Category2", t.Category);
					Assert.Equal("Transaction2", t.Name);
					Assert.Equal("Description2", t.Description);
					Assert.Equal(20.20M, t.Sum);
					Assert.Equal(DateTime.Now.Date, t.DateOfCreation.Date);
					Assert.Null(t.DateOfEditing);
				});
		}

		[Fact]
		public void DisplayTransactions()
		{
			var cells = _page.FindAll("table>tbody>tr>td");
			var totalButtons = _page.FindAll("button");

			Assert.Collection(cells,
				c => Assert.Equal("Income", c.LastElementChild.TextContent),
				c => Assert.Equal("Category1", c.TextContent),
				c => Assert.Equal("Transaction1", c.TextContent),
				c => Assert.Equal("Description1", c.TextContent),
				c => Assert.Equal($"10{decimalSeparator}10", c.TextContent),
				c => Assert.Equal(DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString(), c.TextContent),
				c => Assert.Equal(string.Empty, c.TextContent),
				c => Assert.Equal(3, c.ChildElementCount), // 3 buttons

				c => Assert.Equal("Expenses", c.LastElementChild.TextContent),
				c => Assert.Equal("Category2", c.TextContent),
				c => Assert.Equal("Transaction2", c.TextContent),
				c => Assert.Equal("Description2", c.TextContent),
				c => Assert.Equal($"20{decimalSeparator}20", c.TextContent),
				c => Assert.Equal(DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString(), c.TextContent),
				c => Assert.Equal(string.Empty, c.TextContent),
				c => Assert.Equal(3, c.ChildElementCount) // 3 buttons
			);
			Assert.Equal(7, totalButtons.Count);
			Assert.Equal("Create a new transaction", totalButtons[0].TextContent);
		}

		[Fact]
		public void CreateButton_Click_RedirectToTransactionCreate()
		{
			var createButton = _page.FindAll("button").First(b => b.TextContent == "Create a new transaction");
			createButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}transactions/create", _navigationManager.Uri);
		}
	}
}
