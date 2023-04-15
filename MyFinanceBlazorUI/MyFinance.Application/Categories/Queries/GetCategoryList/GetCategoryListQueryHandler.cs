using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Categories.Queries.GetCategoryList
{
    public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, CategoryListVm>
    {
        private readonly IDataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public GetCategoryListQueryHandler(IDataDbContext dataDbContext, IMapper mapper)
        {
            _dataDbContext = dataDbContext;
            _mapper = mapper;
        }

        public async Task<CategoryListVm> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categoriesQuery = await _dataDbContext.Categories
                .ProjectTo<CategoryListDTO>(_mapper.ConfigurationProvider)
                .OrderBy(comparer => comparer.Name)
                .ToListAsync(cancellationToken);

            return new CategoryListVm { Categories = categoriesQuery };
        }
    }
}
