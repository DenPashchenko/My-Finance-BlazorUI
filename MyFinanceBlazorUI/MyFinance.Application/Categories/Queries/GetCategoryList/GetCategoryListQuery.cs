using MediatR;

namespace MyFinance.Application.Categories.Queries.GetCategoryList
{
    public class GetCategoryListQuery : IRequest<CategoryListVm>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
