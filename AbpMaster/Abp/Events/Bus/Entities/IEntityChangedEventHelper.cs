namespace Abp.Events.Bus.Entities
{
    /// <summary>
    ///     Used to trigger entity change events.
    /// </summary>
    public interface IEntityChangedEventHelper<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        void TriggerEntityCreatedEvent(object entity);
        void TriggerEntityUpdatedEvent(object entity);
        void TriggerEntityDeletedEvent(object entity);
    }
}