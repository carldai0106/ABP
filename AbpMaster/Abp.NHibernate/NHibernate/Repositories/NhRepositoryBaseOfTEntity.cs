using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.NHibernate.Repositories
{
    /// <summary>
    /// A shortcut of <see cref="NhRepositoryBase{TEntity,TPrimaryKey,TTenantId, TUserId}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    public class NhRepositoryBase<TEntity, TTenantId, TUserId> : NhRepositoryBase<TEntity, int, TTenantId, TUserId>, IRepository<TEntity> where TEntity : class, IEntity<int>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Creates a new <see cref="NhRepositoryBase{TEntity,TPrimaryKey,TTenantId, TUserId}"/> object.
        /// </summary>
        /// <param name="sessionProvider">A session provider to obtain session for database operations</param>
        public NhRepositoryBase(ISessionProvider<TTenantId, TUserId> sessionProvider)
            : base(sessionProvider)
        {
        }
    }
}