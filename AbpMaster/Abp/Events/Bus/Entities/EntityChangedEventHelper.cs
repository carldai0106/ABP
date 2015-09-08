using System;
using Abp.Dependency;
using Abp.Domain.Uow;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    ///     Used to trigger entity change events.
    /// </summary>
    public class EntityChangedEventHelper<TTenantId, TUserId> : IEntityChangedEventHelper<TTenantId, TUserId>,
        ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly IUnitOfWorkManager<TTenantId, TUserId> _unitOfWorkManager;

        public EntityChangedEventHelper(IUnitOfWorkManager<TTenantId, TUserId> unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            EventBus = NullEventBus.Instance;
        }

        public IEventBus EventBus { get; set; }

        public void TriggerEntityCreatedEvent(object entity)
        {
            TriggerEntityChangeEvent(typeof (EntityCreatedEventData<>), entity);
        }

        public void TriggerEntityUpdatedEvent(object entity)
        {
            TriggerEntityChangeEvent(typeof (EntityUpdatedEventData<>), entity);
        }

        public void TriggerEntityDeletedEvent(object entity)
        {
            TriggerEntityChangeEvent(typeof (EntityDeletedEventData<>), entity);
        }

        private void TriggerEntityChangeEvent(Type genericEventType, object entity)
        {
            var entityType = entity.GetType();
            var eventType = genericEventType.MakeGenericType(entityType);

            if (_unitOfWorkManager == null || _unitOfWorkManager.Current == null)
            {
                EventBus.Trigger(eventType, (IEventData) Activator.CreateInstance(eventType, entity));
                return;
            }

            _unitOfWorkManager.Current.Completed +=
                (sender, args) => EventBus.Trigger(eventType, (IEventData) Activator.CreateInstance(eventType, entity));
        }
    }
}