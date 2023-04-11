using AutoMapper;
using MediatR;
using MyFinance.Domain;
using MyFinance.Application.Categories.Queries.GetCategoryById;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<CategoryVm> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
            };

            await _dataDbContext.Categories.AddAsync(category, cancellationToken);
            await _dataDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryVm>(category);
        }
    }
}
