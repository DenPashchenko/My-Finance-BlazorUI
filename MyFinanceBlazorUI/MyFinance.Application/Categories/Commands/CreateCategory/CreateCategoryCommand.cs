using MediatR;
using MyFinance.Application.Categories.Queries.GetCategoryById;

namespace MyFinance.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryVm>
    {
        public string Name { get; set; }
    }
}
