using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace CMS.Domain
{
    public interface ICmsRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
    }
}