namespace Abp.Events.Bus.Entities
{
    /// <summary>
    ///     Null-object implementation of <see cref="IEntityChangedEventHelper{TTenantId, TUserId}" />.
    /// </summary>
    public class NullEntityChangedEventHelper<TTenantId, TUserId> : IEntityChangedEventHelper<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        private static readonly NullEntityChangedEventHelper<TTenantId, TUserId> SingletonInstance =
            new NullEntityChangedEventHelper<TTenantId, TUserId>();

        private NullEntityChangedEventHelper()
        {
        }

        /// <summary>
        ///     Gets single instance of <see cref="NullEventBus" /> class.
        /// </summary>
        public static NullEntityChangedEventHelper<TTenantId, TUserId> Instance
        {
            get { return SingletonInstance; }
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityCreatedEvent(object entity)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityUpdatedEvent(object entity)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityDeletedEvent(object entity)
        {
        }
    }
}