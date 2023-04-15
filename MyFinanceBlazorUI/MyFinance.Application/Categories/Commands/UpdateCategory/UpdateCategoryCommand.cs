using MediatR;

namespace MyFinance.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
