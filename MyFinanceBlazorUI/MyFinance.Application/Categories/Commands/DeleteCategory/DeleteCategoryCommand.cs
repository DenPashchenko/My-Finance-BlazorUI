using MediatR;

namespace MyFinance.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
