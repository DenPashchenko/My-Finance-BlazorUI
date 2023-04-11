using MediatR;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Interfaces;
using MyFinance.Domain;

namespace MyFinance.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IDataDbContext _dataDbContext;

        public DeleteCategoryCommandHandler(IDataDbContext dataDbContext) => _dataDbContext = dataDbContext;

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var dbCategory = await _dataDbContext.Categories.FindAsync(request.Id);
            if (dbCategory == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            _dataDbContext.Categories.Remove(dbCategory);
            await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
