using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    ///     A shortcut of <see cref="FullAuditedEntityDto{TPrimaryKey}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    [Serializable]
    public class FullAuditedEntityDto<TUserId> : FullAuditedEntityDto<int, TUserId> where TUserId : struct
    {
    }
}