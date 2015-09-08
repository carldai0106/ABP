using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     A shortcut of <see cref="AuditedEntity" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    [Serializable]
    public abstract class AuditedEntity : AuditedEntity<int, int>
    {
    }
}