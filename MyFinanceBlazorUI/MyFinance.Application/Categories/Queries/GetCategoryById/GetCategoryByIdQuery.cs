using MediatR;

namespace MyFinance.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CategoryVm>
    {
        public int Id { get; set; }
    }
}
