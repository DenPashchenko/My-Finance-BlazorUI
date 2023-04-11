using AutoMapper;
using MyFinance.Application.Common.Mappings;
using MyFinance.Persistence;
using MyFinance.Application.Interfaces;

namespace MyFinance.UnitTests.QueriesAndCommandsTests.Common
{
    public class QueryTestFixture : TestFixtureBase
    {
        public DataDbContext context;
        public IMapper mapper;

        public QueryTestFixture()
        {
            context = MyFinanceContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(IDataDbContext).Assembly));
            });
            mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            MyFinanceContextFactory.Destroy(context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
