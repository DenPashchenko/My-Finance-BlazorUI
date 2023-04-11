using AutoMapper;
using MediatR;
using MyFinance.Application.Common.Exceptions;
using MyFinance.Application.Interfaces;
using MyFinance.Domain;

namespace MyFinance.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<CategoryVm> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _dataDbContext.Categories.FindAsync(request.Id);
            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            return _mapper.Map<CategoryVm>(category);
        }
    }
}
