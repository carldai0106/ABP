using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    ///     A shortcut of <see cref="AuditedEntityDto{TPrimaryKey}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    [Serializable]
    public abstract class AuditedEntityDto<TUserId> : AuditedEntityDto<int, TUserId> where TUserId : struct
    {
    }
}