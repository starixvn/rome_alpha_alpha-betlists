using Alpha.BetLists.StaticData.Configurations;
using Alpha.IntegrationTest.Mapper;
using Alpha.IntegrationTest.Repository;
using Fanex.Data.Repository;
using SimpleInjector;

namespace Alpha.BetLists.StaticData.AppStart
{
    public static class DependencyRegister
    {
        public static void Register(Container container)
        {
            container.Register<IRepositoryMapper, RepositoryMapper>(Lifestyle.Singleton);
            container.Register<IDynamicRepository, StaticRepository>(Lifestyle.Singleton);
        }
    }
}
