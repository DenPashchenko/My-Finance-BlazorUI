using AutoMapper;
using MyFinance.Application.Common.Mappings;
using MyFinance.Application.Interfaces;
using MyFinance.Persistence;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Common
{
    public abstract class TestFixtureBase : IDisposable
    {
        public DataDbContext _context;
        public IMapper _mapper;

        public TestFixtureBase()
        {
            _context = MyFinanceContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(IDataDbContext).Assembly));
            });
            _mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            MyFinanceContextFactory.Destroy(_context);
        }
    }
}
