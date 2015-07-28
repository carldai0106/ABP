﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using EntityFramework.DynamicFilters;

namespace Abp.EntityFramework
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class AbpDbContext<TTenantId, TUserId> : DbContext, IShouldInitialize
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Used to get current session values.
        /// </summary>
        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }

        /// <summary>
        /// Used to trigger entity change events.
        /// </summary>
        public IEntityChangedEventHelper<TTenantId, TUserId> EntityChangedEventHelper { get; set; }

        public bool IsTest { get; set; }

        /// <summary>
        /// Constructor.
        /// Uses <see cref="IAbpStartupConfiguration.DefaultNameOrConnectionString"/> as connection string.
        /// </summary>
        protected AbpDbContext()
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpDbContext(DbCompiledModel model)
            : base(model)
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper<TTenantId, TUserId>.Instance;
        }

        public virtual void Initialize()
        {
            Database.Initialize(false);
            this.SetFilterScopedParameterValue(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId ?? default(TTenantId));
            this.SetFilterScopedParameterValue(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Filter(AbpDataFilters.SoftDelete, (ISoftDelete d) => d.IsDeleted, false);

            var parMust1 = Expression.Parameter(typeof(IMustHaveTenant<TTenantId>), "t");
            var parMust2 = Expression.Parameter(typeof(TTenantId), "tenantId");

            var expMust = Expression.Lambda<Func<IMustHaveTenant<TTenantId>, TTenantId, bool>>(
                    Expression.Equal(Expression.PropertyOrField(parMust1, "TenantId"), parMust2), parMust1, parMust2
                );

            modelBuilder.Filter(AbpDataFilters.MustHaveTenant, expMust, default(TTenantId));


            var parMay1 = Expression.Parameter(typeof(IMayHaveTenant<TTenantId>), "t");
            var parMay2 = Expression.Parameter(typeof(TTenantId?), "tenantId");

            var expMay = Expression.Lambda<Func<IMayHaveTenant<TTenantId>, TTenantId?, bool>>(
                    Expression.Equal(Expression.PropertyOrField(parMay1, "TenantId"), parMay2), parMay1, parMay2
                );

            modelBuilder.Filter(AbpDataFilters.MayHaveTenant, expMay, default(TTenantId));
        }
       

        public override int SaveChanges()
        {
            ApplyAbpConcepts();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ApplyAbpConcepts();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ApplyAbpConcepts()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditProperties(entry);
                        CheckAndSetTenantIdProperty(entry);
                        EntityChangedEventHelper.TriggerEntityCreatedEvent(entry.Entity);
                        break;
                    case EntityState.Modified:
                        PreventSettingCreationAuditProperties(entry);
                        CheckAndSetTenantIdProperty(entry);
                        SetModificationAuditProperties(entry);

                        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                        {
                            if (entry.Entity is IDeletionAudited<TUserId>)
                            {
                                SetDeletionAuditProperties(entry.Entity.As<IDeletionAudited<TUserId>>());
                            }

                            EntityChangedEventHelper.TriggerEntityDeletedEvent(entry.Entity);
                        }
                        else
                        {
                            EntityChangedEventHelper.TriggerEntityUpdatedEvent(entry.Entity);
                        }

                        break;
                    case EntityState.Deleted:
                        PreventSettingCreationAuditProperties(entry);
                        HandleSoftDelete(entry);
                        EntityChangedEventHelper.TriggerEntityDeletedEvent(entry.Entity);
                        break;
                }
            }
        }

        protected virtual void CheckAndSetTenantIdProperty(DbEntityEntry entry)
        {
            if (entry.Entity is IMustHaveTenant<TTenantId>)
            {
                CheckAndSetMustHaveTenant(entry);
            }
            else if (entry.Entity is IMayHaveTenant<TTenantId>)
            {
                CheckMayHaveTenant(entry);
            }
        }

        protected virtual void CheckAndSetMustHaveTenant(DbEntityEntry entry)
        {
            var entity = entry.Cast<IMustHaveTenant<TTenantId>>().Entity;

            if (!this.IsFilterEnabled(AbpDataFilters.MustHaveTenant))
            {
                if (AbpSession.TenantId != null && entity.TenantId.Equals(default(TTenantId)))
                {
                    entity.TenantId = AbpSession.GetTenantId();
                }

                return;
            }

            var currentTenantId =
               Convert.ChangeType(
                   this.GetFilterParameterValue(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId),
                   typeof(TTenantId));

            if (IsTest)
                return;

            if (currentTenantId.Equals(default(TTenantId)))
            {
                throw new DbEntityValidationException("Can not save a IMustHaveTenant entity while MustHaveTenant filter is enabled and current filter parameter value is not set (Probably, no tenant user logged in)!");
            }

            if (entity.TenantId.Equals(default(TTenantId)))
            {
                entity.TenantId = (TTenantId)currentTenantId;
            }
            else if (!entity.TenantId.Equals(currentTenantId) && !entity.TenantId.Equals(AbpSession.TenantId))
            {
                throw new DbEntityValidationException("Can not set IMustHaveTenant.TenantId to a different value than the current filter parameter value or IAbpSession.TenantId while MustHaveTenant filter is enabled!");
            }
        }

        

        protected virtual void CheckMayHaveTenant(DbEntityEntry entry)
        {
            if (!this.IsFilterEnabled(AbpDataFilters.MayHaveTenant))
            {
                return;
            }

            var currentTenantId =
                Convert.ChangeType(
                    this.GetFilterParameterValue(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId),
                    typeof (TTenantId));

            var entity = entry.Cast<IMayHaveTenant<TTenantId>>().Entity;

            if (IsTest)
                return;

            if (!entity.TenantId.Equals(currentTenantId) && !entity.TenantId.Equals(AbpSession.TenantId))
            {
                throw new DbEntityValidationException("Can not set TenantId to a different value than the current filter parameter value or IAbpSession.TenantId while MayHaveTenant filter is enabled!");
            }
        }

        protected virtual void SetCreationAuditProperties(DbEntityEntry entry)
        {
            if (entry.Entity is IHasCreationTime)
            {
                entry.Cast<IHasCreationTime>().Entity.CreationTime = Clock.Now;
            }

            if (entry.Entity is ICreationAudited<TUserId>)
            {
                entry.Cast<ICreationAudited<TUserId>>().Entity.CreatorUserId = AbpSession.UserId;
            }
        }

        protected virtual void PreventSettingCreationAuditProperties(DbEntityEntry entry)
        {
            //TODO@Halil: Implement this when tested well (Issue #49)
            //if (entry.Entity is IHasCreationTime && entry.Cast<IHasCreationTime>().Property(e => e.CreationTime).IsModified)
            //{
            //    throw new DbEntityValidationException(string.Format("Can not change CreationTime on a modified entity {0}", entry.Entity.GetType().FullName));
            //}

            //if (entry.Entity is ICreationAudited && entry.Cast<ICreationAudited>().Property(e => e.CreatorUserId).IsModified)
            //{
            //    throw new DbEntityValidationException(string.Format("Can not change CreatorUserId on a modified entity {0}", entry.Entity.GetType().FullName));
            //}
        }

        protected virtual void SetModificationAuditProperties(DbEntityEntry entry)
        {
            if (entry.Entity is IModificationAudited<TUserId>)
            {
                var auditedEntry = entry.Cast<IModificationAudited<TUserId>>();

                auditedEntry.Entity.LastModificationTime = Clock.Now;
                auditedEntry.Entity.LastModifierUserId = AbpSession.UserId;
            }
        }

        protected virtual void HandleSoftDelete(DbEntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            var softDeleteEntry = entry.Cast<ISoftDelete>();

            softDeleteEntry.State = EntityState.Unchanged;
            softDeleteEntry.Entity.IsDeleted = true;

            if (entry.Entity is IDeletionAudited<TUserId>)
            {
                SetDeletionAuditProperties(entry.Cast<IDeletionAudited<TUserId>>().Entity);
            }
        }

        protected virtual void SetDeletionAuditProperties(IDeletionAudited<TUserId> entity)
        {
            entity.DeletionTime = Clock.Now;
            entity.DeleterUserId = AbpSession.UserId;
        }
    }

   
}