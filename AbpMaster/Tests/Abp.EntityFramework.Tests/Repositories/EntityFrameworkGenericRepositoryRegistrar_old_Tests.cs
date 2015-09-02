using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFramework.Repositories;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Abp.Tests;
using Castle.MicroKernel.Registration;
using Shouldly;
using Xunit;

namespace Abp.EntityFramework.Tests.Repositories
{
    public class EntityFrameworkGenericRepositoryRegistrar_old_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void Should_Resolve_Generic_Repositories()
        {
            var fakeDbContextProvider = NSubstitute.Substitute.For<IDbContextProvider<MyDbContext<int, long>, int, long>>();

            LocalIocManager.IocContainer.Register(
                Component.For<ITypeFinder>().ImplementedBy<TypeFinder>(),
                Component.For<IDbContextProvider<MyDbContext<int, long>, int, long>>().UsingFactoryMethod(() => fakeDbContextProvider)
                );

            var typeFinder = LocalIocManager.Resolve<ITypeFinder>();
            var finder = typeFinder as TypeFinder;
            finder.AssemblyFinder = new AssembleFinder();


            var entities = finder.Find(
               x => (
                   typeof(IEntity).IsAssignableFrom(x) ||
                   x.IsInheritsOrImplements(typeof(IEntity<>))
                   )
               );

            //EntityFrameworkGenericRepositoryRegistrar.RegisterGenericRepositories<int, long>(finder,
            //    LocalIocManager);

            var dbContextType = typeof(MyDbContext<int, long>);
            EntityFrameworkGenericRepositoryRegistrar.RegisterForDbContext<int, long>(dbContextType, LocalIocManager);

            var entity1Repository = LocalIocManager.Resolve<IRepository<MyEntity1>>();
            entity1Repository.ShouldNotBe(null);

            var entity1RepositoryWithPk = LocalIocManager.Resolve<IRepository<MyEntity1, int>>();
            entity1RepositoryWithPk.ShouldNotBe(null);

            var entity2Repository = LocalIocManager.Resolve<IRepository<MyEntity2, long>>();
            entity2Repository.ShouldNotBe(null);
        }
       
    }

    public class AssembleFinder : IAssemblyFinder
    {
        public List<Assembly> GetAllAssemblies()
        {
            var list = System.AppDomain.CurrentDomain.GetAssemblies();
            var types = list.Where(x => x.FullName.Contains("Abp.EntityFramework.Tests"));

            return types.ToList();
        }
    }

    public class MyDbContext<TTenantId, TUserId> : MyBaseDbContext<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public DbSet<MyEntity2> MyEntities2 { get; set; }
    }

    public abstract class MyBaseDbContext<TTenantId, TUserId> : AbpDbContext<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public IDbSet<MyEntity1> MyEntities1 { get; set; }
    }

    public class MyEntity1 : Entity
    {

    }

    public class MyEntity2 : Entity<long>
    {

    }
}
