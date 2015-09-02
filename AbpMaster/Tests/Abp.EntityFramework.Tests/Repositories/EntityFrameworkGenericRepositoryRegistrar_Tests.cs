using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
    public class EntityFrameworkGenericRepositoryRegistrar_Tests: TestBaseWithLocalIocManager
    {
        public EntityFrameworkGenericRepositoryRegistrar_Tests()
        {
            var fakeMainDbContextProvider = NSubstitute.Substitute.For<IDbContextProvider<MyMainDbContext<int,long>, int, long>>();
            var fakeModuleDbContextProvider = NSubstitute.Substitute.For<IDbContextProvider<MyModuleDbContext<int, long>, int, long>>();

            LocalIocManager.IocContainer.Register(
                Component.For<ITypeFinder>().ImplementedBy<TypeFinder>(),
                Component.For<IDbContextProvider<MyMainDbContext<int, long>, int, long>>().UsingFactoryMethod(() => fakeMainDbContextProvider),
                Component.For<IDbContextProvider<MyModuleDbContext<int, long>, int, long>>().UsingFactoryMethod(() => fakeModuleDbContextProvider)
                );

            var typeFinder = LocalIocManager.Resolve<ITypeFinder>();
            var finder = typeFinder as TypeFinder;
            finder.AssemblyFinder = new Repositories.AssembleFinder();


            var entities = finder.Find(
               x => (
                   typeof(IEntity).IsAssignableFrom(x) ||
                   x.IsInheritsOrImplements(typeof(IEntity<>))
                   )
               );

            EntityFrameworkGenericRepositoryRegistrar.RegisterGenericRepositories<int, long>(finder, LocalIocManager);
            EntityFrameworkGenericRepositoryRegistrar.RegisterGenericRepositories<int, long>(typeof(MyMainDbContext<int, long>), entities, LocalIocManager);

            //EntityFrameworkGenericRepositoryRegistrar.RegisterForDbContext<int, long>(typeof(MyModuleDbContext<int, long>), LocalIocManager);
            //EntityFrameworkGenericRepositoryRegistrar.RegisterForDbContext<int, long>(typeof(MyMainDbContext<int, long>), LocalIocManager);
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

        [Fact]
        public void Should_Resolve_Generic_Repositories()
        {
            //Entity 1 (with default PK)
            var entity1Repository = LocalIocManager.Resolve<IRepository<MyEntity11>>();
            entity1Repository.ShouldNotBe(null);
            (entity1Repository is EfRepositoryBase<MyMainDbContext<int, long>, MyEntity11, int, long>).ShouldBe(true);

            //Entity 1 (with specified PK)
            var entity1RepositoryWithPk = LocalIocManager.Resolve<IRepository<MyEntity11, int>>();
            entity1RepositoryWithPk.ShouldNotBe(null);
            (entity1RepositoryWithPk is EfRepositoryBase<MyMainDbContext<int, long>, MyEntity11, int, int, long>).ShouldBe(true);

            //Entity 2
            var entity2Repository = LocalIocManager.Resolve<IRepository<MyEntity22, long>>();
            (entity2Repository is EfRepositoryBase<MyMainDbContext<int, long>, MyEntity22, long, int, long>).ShouldBe(true);
            entity2Repository.ShouldNotBe(null);

            //Entity 3
            var entity3Repository = LocalIocManager.Resolve<IMyModuleRepository<MyEntity33, Guid>>();
            (entity3Repository is EfRepositoryBase<MyModuleDbContext<int, long>, MyEntity33, Guid, int, long>).ShouldBe(true);
            entity3Repository.ShouldNotBe(null);
        }

    }


    public class MyMainDbContext<TTenantId, TUserId> : MyBaseDbContext1<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public virtual DbSet<MyEntity22> MyEntities2 { get; set; }
    }

     [AutoRepositoryTypes(
            typeof(IMyModuleRepository<>),
            typeof(IMyModuleRepository<,>),
            typeof(MyModuleRepositoryBase<>),
            typeof(MyModuleRepositoryBase<,,,>)
            )]
    public class MyModuleDbContext<TTenantId, TUserId> : MyBaseDbContext1<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public virtual DbSet<MyEntity33> MyEntities3 { get; set; }
    }

     public abstract class MyBaseDbContext1<TTenantId, TUserId> : AbpDbContext<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public virtual IDbSet<MyEntity11> MyEntities1 { get; set; }
    }

    public class MyEntity11 : Entity
    {

    }

    public class MyEntity22 : Entity<long>
    {

    }

    public class MyEntity33 : Entity<Guid>
    {

    }

    public interface IMyModuleRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {

    }

    public interface IMyModuleRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

    }

    public class MyModuleRepositoryBase<TEntity, TPrimaryKey, TTenantId, TUserId> : EfRepositoryBase<MyModuleDbContext<TTenantId, TUserId>, TEntity, TPrimaryKey, TTenantId, TUserId>, IMyModuleRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TTenantId : struct
        where TUserId : struct
    {
        public MyModuleRepositoryBase(IDbContextProvider<MyModuleDbContext<TTenantId, TUserId>, TTenantId, TUserId> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public class MyModuleRepositoryBase<TEntity> : MyModuleRepositoryBase<TEntity, int, int, long>, IMyModuleRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public MyModuleRepositoryBase(IDbContextProvider<MyModuleDbContext<int, long>, int, long> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
