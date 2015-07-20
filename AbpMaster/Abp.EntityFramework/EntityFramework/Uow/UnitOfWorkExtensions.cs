using System;
using System.Data.Entity;
using Abp.Domain.Uow;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext as a part of active unit of work.
        /// This method can be called when current unit of work is an <see cref="EfUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
        /// <typeparam name="TTenantId"></typeparam>
        /// <typeparam name="TUserId"></typeparam>
        /// <param name="unitOfWork">Current (active) unit of work</param>
        public static TDbContext GetDbContext<TDbContext, TTenantId, TUserId>(this IActiveUnitOfWork unitOfWork) 
            where TDbContext : DbContext
            where TTenantId : struct
            where TUserId : struct

        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is EfUnitOfWork<TTenantId, TUserId>))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfUnitOfWork<TTenantId, TUserId>).FullName, "unitOfWork");
            }

            return (unitOfWork as EfUnitOfWork<TTenantId, TUserId>).GetOrCreateDbContext<TDbContext>();
        }
    }
}