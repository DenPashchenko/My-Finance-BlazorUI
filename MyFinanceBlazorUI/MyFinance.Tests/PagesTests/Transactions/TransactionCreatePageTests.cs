using AutoMapper;
using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using MyFinance.WebBlazorUI.Models.TransactionDtos;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.WebBlazorUI.Pages.TransactionPages;
using MyFinance.Application.Categories.Queries.GetCategoryList;
using MyFinance.WebBlazorUI.Properties;

namespace MyFinance.UnitTests.PagesTests.Transactions
{
	public class TransactionCreatePageTests
	{
		private readonly TestContext _testContext;
		private IRenderedComponent<TransactionCreate> _page;
		private readonly FakeNavigationManager _navigationManager;
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<IMapper> _mockMapper;
		private CreateTransactionDto _createTransactionDto;
		private CategoryListVm _categoryListVm;

		public TransactionCreatePageTests()
		{
			_testContext = new TestContext();
			_mockMediator = new Mock<IMediator>();
			_mockMapper = new Mock<IMapper>();
			_categoryListVm = new();

			var categories = new List<CategoryListDTO>
			{
				new(){ Id = 1, Name = "Category1"},
				new(){ Id = 2, Name = "Category2"},
			};
			_categoryListVm.Categories = categories;

			_createTransactionDto = new()
			{
				TransactionType = Domain.Enums.TransactionType.Income,
				CategoryId = 1,
				Name = "Transaction1",
				Description = "Description1",
				Sum = 10.10M
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetCategoryListQuery>(), default)).ReturnsAsync(_categoryListVm);
			_testContext.Services.AddSingleton(_mockMediator.Object);
			_testContext.Services.AddSingleton(_mockMapper.Object);

			_page = _testContext.RenderComponent<TransactionCreate>();

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void SaveButtonClick_ValidInput_RedirectToTransactions()
		{
			var transactionTypeInputSelect = _page.Find("#transactionType");
			var categoryInputSelect = _page.Find("#category");
			var nameInput = _page.Find("#name");
			var descriptionInput = _page.Find("#description");
			var sumInput = _page.Find("#sum");

			transactionTypeInputSelect.Change(_createTransactionDto.TransactionType);
			categoryInputSelect.Change(_createTransactionDto.CategoryId);
			nameInput.Change(_createTransactionDto.Name);
			descriptionInput.Change(_createTransactionDto.Description);
			sumInput.Change(_createTransactionDto.Sum);

			var saveButton = _page.FindAll("button").First(b => b.TextContent == "Save");

			saveButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}transactions", _navigationManager.Uri);
		}

		[Fact]
		public void SaveButtonClick_InvalidTransactionName_NotRedirected()
		{
			var invalidName = "n"; // 1 char is not valid for a transaction name

			var transactionTypeInputSelect = _page.Find("#transactionType");
			var categoryInputSelect = _page.Find("#category");
			var nameInput = _page.Find("#name");
			var descriptionInput = _page.Find("#description");
			var sumInput = _page.Find("#sum");

			transactionTypeInputSelect.Change(_createTransactionDto.TransactionType);
			categoryInputSelect.Change(_createTransactionDto.CategoryId);
			nameInput.Change(invalidName);
			descriptionInput.Change(_createTransactionDto.Description);
			sumInput.Change(_createTransactionDto.Sum);

			var saveButton = _page.FindAll("button").First(b => b.TextContent == "Save");

			saveButton.Click();

			Assert.Equal(_navigationManager.BaseUri, _navigationManager.Uri);
		}

		[Fact]
		public void SaveButtonClick_InvalidTransactionSum_NotRedirected()
		{
			decimal invalidSum = 0; // The sum that is less than 0.01 is not valid for a transaction sum

			var transactionTypeInputSelect = _page.Find("#transactionType");
			var categoryInputSelect = _page.Find("#category");
			var nameInput = _page.Find("#name");
			var descriptionInput = _page.Find("#description");
			var sumInput = _page.Find("#sum");

			transactionTypeInputSelect.Change(_createTransactionDto.TransactionType);
			categoryInputSelect.Change(_createTransactionDto.CategoryId);
			nameInput.Change(_createTransactionDto.Name);
			descriptionInput.Change(_createTransactionDto.Description);
			sumInput.Change(invalidSum);

			var saveButton = _page.FindAll("button").First(b => b.TextContent == "Save");

			saveButton.Click();

			Assert.Equal(_navigationManager.BaseUri, _navigationManager.Uri);
		}

		[Fact]
		public void OnInitializedAsync_CategoryListVmIsEmpty_ThrowsInvalidOperationException()
		{
			_categoryListVm = new CategoryListVm { Categories = new List<CategoryListDTO>() };
			_mockMediator.Setup(m => m.Send(It.IsAny<GetCategoryListQuery>(), default)).ReturnsAsync(_categoryListVm);

			var exception = Assert.Throws<InvalidOperationException>(() =>
			{
				_testContext.RenderComponent<TransactionCreate>();
			});
			Assert.Equal(Resources.DefineCategoryFirst, exception.Message);
		}
	}
}
