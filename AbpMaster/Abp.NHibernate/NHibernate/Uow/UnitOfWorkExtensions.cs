using System;
using Abp.Domain.Uow;
using NHibernate;

namespace Abp.NHibernate.Uow
{
    internal static class UnitOfWorkExtensions
    {
        public static ISession GetSession<TTenantId, TUserId>(this IActiveUnitOfWork unitOfWork)
            where TTenantId : struct
            where TUserId : struct
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is NhUnitOfWork<TTenantId, TUserId>))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(NhUnitOfWork<,>).FullName, "unitOfWork");
            }

            return (unitOfWork as NhUnitOfWork<TTenantId, TUserId>).Session;
        }
    }
}