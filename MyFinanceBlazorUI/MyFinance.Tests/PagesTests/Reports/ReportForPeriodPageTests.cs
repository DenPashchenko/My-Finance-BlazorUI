using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using MyFinance.Application.Transactions.Queries.GetTransactionList;
using MyFinance.WebBlazorUI.Pages.ReportsPages;
using MyFinance.Application.Reports.Queries.ReportForPeriod;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.UnitTests.PagesTests.Reports
{
	public class ReportForPeriodPageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly IRenderedComponent<ReportForPeriod> _page;
		private readonly FakeNavigationManager _navigationManager;
		private ReportForPeriodVm _reportVm;

		public ReportForPeriodPageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_reportVm = new ReportForPeriodVm
			{
				TotalIncome = 10.10m,
				TotalExpences = 20.20m,
				ForPeriod = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + " - " + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
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

			_mockMediator.Setup(m => m.Send(It.IsAny<GetReportForPeriodQuery>(), default)).ReturnsAsync(_reportVm);
			_testContext.Services.AddSingleton(_mockMediator.Object);

			_page = _testContext.RenderComponent<ReportForPeriod>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchDateBeforeReporting()
		{
			var startDate = _page.Instance.Model.StartDate;
			var endDate = _page.Instance.Model.EndDate;

			Assert.Equal(DateTime.Now.Date, startDate);
			Assert.Equal(DateTime.Now.Date, endDate);
		}

		[Fact]
		public void DisplayDateBeforeReporting()
		{
			var startDate = _page.Find("#startDate").GetAttribute("value");
			var endDate = _page.Find("#endDate").GetAttribute("value");
			var totalButtons = _page.FindAll("button");

			Assert.Equal(DateTime.Now.Date.ToString("yyyy-MM-dd"), startDate);
			Assert.Equal(DateTime.Now.Date.ToString("yyyy-MM-dd"), endDate);
			Assert.Equal(1, totalButtons.Count);
			Assert.Equal("Get the report", totalButtons[0].TextContent);
		}

		[Fact]
		public void ButtonClick_ValidDates_GetReport()
		{
			var startDate = _page.Find("#startDate");
			var endDate = _page.Find("#endDate");
			startDate.Change(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
			endDate.Change(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
			var button = _page.Find("button");

			button.Click();

			Assert.Equal(_reportVm.TotalIncome, _page.Instance.ReportForPeriodVm.TotalIncome);
			Assert.Equal(_reportVm.TotalExpences, _page.Instance.ReportForPeriodVm.TotalExpences);
			Assert.Equal(_reportVm.ForPeriod, _page.Instance.ReportForPeriodVm.ForPeriod);
			Assert.Equal(_reportVm.Transactions, _page.Instance.ReportForPeriodVm.Transactions);
		}

		[Fact]
		public void ButtonClick_InvalidEndDate_ThrowsValidationException()
		{
			var endDate = _page.Find("#endDate");
			endDate.Change(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
			var button = _page.Find("button");

			var exception = Assert.Throws<ValidationException>(() =>
			{
				button.Click();
			});
			Assert.Equal(Application.Properties.Resources.InvalidDates, exception.Message);
		}

		[Fact]
		public void ButtonClick_InvalidStartDate_ValidationFails()
		{
			var startDate = _page.Find("#startDate");
			var endDate = _page.Find("#endDate");
			startDate.Change(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
			endDate.Change(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
			var editContext = _page.Instance.EditContext;
			var button = _page.Find("button");

			button.Click();
			var validationMessages = editContext.GetValidationMessages();

			Assert.Contains(WebBlazorUI.Properties.Resources.StartDateError, validationMessages);
		}
	}
}
