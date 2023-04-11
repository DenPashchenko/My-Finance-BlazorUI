using MediatR;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Interfaces;
using MyFinance.Domain;

namespace MyFinance.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IDataDbContext _dataDbContext;

        public UpdateCategoryCommandHandler(IDataDbContext dataDbContext) => _dataDbContext = dataDbContext;

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dbCategory = await _dataDbContext.Categories.FindAsync(request.Id);
            if (dbCategory == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            dbCategory.Name = request.Name;
            await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
