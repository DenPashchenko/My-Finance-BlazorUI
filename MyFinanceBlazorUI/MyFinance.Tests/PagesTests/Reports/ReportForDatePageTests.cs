using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using MyFinance.Application.Reports.Queries.ReportForDate;
using MyFinance.WebBlazorUI.Pages.ReportsPages;
using MyFinance.Application.Transactions.Queries.GetTransactionList;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using MyFinance.Application.Properties;

namespace MyFinance.UnitTests.PagesTests.Reports
{
	public class ReportForDatePageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly IRenderedComponent<ReportForDate> _page;
		private readonly FakeNavigationManager _navigationManager;
		private ReportForDateVm _reportVm;

		public ReportForDatePageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_reportVm = new ReportForDateVm
			{
				TotalIncome = 10.10m,
				TotalExpences = 20.20m,
				ForDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
				Transactions = new List<TransactionListDto>
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
				}
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetReportForDateQuery>(), default)).ReturnsAsync(_reportVm);
			_testContext.Services.AddSingleton(_mockMediator.Object);

			_page = _testContext.RenderComponent<ReportForDate>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchDateBeforeReporting()
		{
			var date = _page.Instance.Date;

			Assert.Equal(DateTime.Now.Date, date);
		}

		[Fact]
		public void DisplayDateBeforeReporting()
		{
			var date = _page.Find("#date").GetAttribute("value");
			var totalButtons = _page.FindAll("button");

			Assert.Equal(DateTime.Now.Date.ToString("yyyy-MM-dd"), date);
			Assert.Equal(1, totalButtons.Count);
			Assert.Equal("Get the report", totalButtons[0].TextContent);
		}

		[Fact]
		public void ButtonClick_ValidDate_GetReport()
		{
			var date = _page.Find("#date");
			date.Change(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
			var button = _page.Find("button");

			button.Click();

			Assert.Equal(_reportVm.TotalIncome, _page.Instance.ReportForDateVm.TotalIncome);
			Assert.Equal(_reportVm.TotalExpences, _page.Instance.ReportForDateVm.TotalExpences);
			Assert.Equal(_reportVm.ForDate, _page.Instance.ReportForDateVm.ForDate);
			Assert.Equal(_reportVm.Transactions, _page.Instance.ReportForDateVm.Transactions);
		}

		[Fact]
		public void ButtonClick_InvalidDate_ThrowsValidationException()
		{
			var date = _page.Find("#date");
			date.Change(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
			var button = _page.Find("button");

			var exception = Assert.Throws<ValidationException>(() =>
			{
				button.Click();
			});
			Assert.Equal(Resources.InvalidDate, exception.Message);
		}
	}
}
