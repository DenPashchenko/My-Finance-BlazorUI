using AutoMapper;
using Bunit.TestDoubles;
using Bunit;
using MediatR;
using Moq;
using MyFinance.WebBlazorUI.Pages.TransactionPages;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Transactions.Queries.GetTransactionById;
using MyFinance.WebBlazorUI.Models.TransactionDtos;
using MyFinance.Application.Categories.Queries.GetCategoryList;
using MyFinance.Application.Transactions.Commands.UpdateTransaction;

namespace MyFinance.UnitTests.PagesTests.Transactions
{
	public class TransactionEditPageTests
	{
		private readonly TestContext _testContext;
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<IMapper> _mockMapper;
		private readonly IRenderedComponent<TransactionEdit> _page;
		private readonly FakeNavigationManager _navigationManager;
		private TransactionVm _transactionVm;
		private UpdateTransactionDto _updateTransactionDto;
		private CategoryListVm _categoryListVm;

		public TransactionEditPageTests()
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

			_updateTransactionDto = new()
			{
				TransactionType = Domain.Enums.TransactionType.Expenses,
				CategoryId = 2,
				Name = "Transaction2",
				Description = "Description2",
				Sum = 20.20M
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetTransactionByIdQuery>(), default)).ReturnsAsync(_transactionVm);
			_mockMediator.Setup(m => m.Send(It.IsAny<GetCategoryListQuery>(), default)).ReturnsAsync(_categoryListVm);
			_mockMediator.Setup(m => m.Send(It.IsAny<UpdateTransactionCommand>(), default)).Returns(Task.CompletedTask);

			_mockMapper.Setup(m => m.Map<UpdateTransactionCommand>(It.IsAny<UpdateTransactionDto>())).Returns(new UpdateTransactionCommand());

			_testContext.Services.AddSingleton(_mockMediator.Object);
			_testContext.Services.AddSingleton(_mockMapper.Object);

			_page = _testContext.RenderComponent<TransactionEdit>(parameters => parameters.Add(p => p.Id, 1));

			_navigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
		}

		[Fact]
		public void FetchTransactionByIdBeforeEditing()
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
		public void DisplayTransactionByIdBeforeEditing()
		{
			var typeValue = _page.Find("#transactionType").GetAttribute("value");
			var categoryValue = _page.Find("#category").GetAttribute("value");
			var nameValue = _page.Find("#name").GetAttribute("value");
			var descriptionValue = _page.Find("#description").GetAttribute("value");
			var sumValue = _page.Find("#sum").GetAttribute("value");

			var totalButtons = _page.FindAll("button");

			Assert.Equal("Income", typeValue);
			Assert.Equal("1", categoryValue);
			Assert.Equal("Transaction1", nameValue);
			Assert.Equal("Description1", descriptionValue);
			Assert.Equal("10.10", sumValue);

			Assert.Equal(2, totalButtons.Count);
			Assert.Equal("Update", totalButtons[0].TextContent);
			Assert.Equal("Reset", totalButtons[1].TextContent);
		}

		[Fact]
		public void UpdateButtonClick_ValidInput_RedirectToCategories()
		{
			var transactionTypeInputSelect = _page.Find("#transactionType");
			var categoryInputSelect = _page.Find("#category");
			var nameInput = _page.Find("#name");
			var descriptionInput = _page.Find("#description");
			var sumInput = _page.Find("#sum");

			transactionTypeInputSelect.Change(_updateTransactionDto.TransactionType);
			categoryInputSelect.Change(_updateTransactionDto.CategoryId);
			nameInput.Change(_updateTransactionDto.Name);
			descriptionInput.Change(_updateTransactionDto.Description);
			sumInput.Change(_updateTransactionDto.Sum);

			var updateButton = _page.FindAll("button").First(b => b.TextContent == "Update");

			updateButton.Click();

			Assert.Equal($"{_navigationManager.BaseUri}transactions", _navigationManager.Uri);
		}

		[Fact]
		public void UpdateButtonClick_InvalidTransactionName_NotRedirected()
		{
			var invalidName = "n"; // 1 char is not valid for a transaction name

			var transactionTypeInputSelect = _page.Find("#transactionType");
			var categoryInputSelect = _page.Find("#category");
			var nameInput = _page.Find("#name");
			var descriptionInput = _page.Find("#description");
			var sumInput = _page.Find("#sum");

			transactionTypeInputSelect.Change(_updateTransactionDto.TransactionType);
			categoryInputSelect.Change(_updateTransactionDto.CategoryId);
			nameInput.Change(invalidName);
			descriptionInput.Change(_updateTransactionDto.Description);
			sumInput.Change(_updateTransactionDto.Sum);

			var updateButton = _page.FindAll("button").First(b => b.TextContent == "Update");

			updateButton.Click();

			Assert.Equal(_navigationManager.BaseUri, _navigationManager.Uri);

		}

		[Fact]
		public void UpdateButtonClick_InvalidTransactionSum_NotRedirected()
		{
			decimal invalidSum = 0; // The sum that is less than 0.01 is not valid for a transaction sum

			var transactionTypeInputSelect = _page.Find("#transactionType");
			var categoryInputSelect = _page.Find("#category");
			var nameInput = _page.Find("#name");
			var descriptionInput = _page.Find("#description");
			var sumInput = _page.Find("#sum");

			transactionTypeInputSelect.Change(_updateTransactionDto.TransactionType);
			categoryInputSelect.Change(_updateTransactionDto.CategoryId);
			nameInput.Change(_updateTransactionDto.Name);
			descriptionInput.Change(_updateTransactionDto.Description);
			sumInput.Change(invalidSum);

			var updateButton = _page.FindAll("button").First(b => b.TextContent == "Update");

			updateButton.Click();

			Assert.Equal(_navigationManager.BaseUri, _navigationManager.Uri);
		}
	}
}
