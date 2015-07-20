using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Abp.TestBase.SampleApplication
{
    [Table("Messages")]
    public class Message : Entity, IMayHaveTenant<int>
    {
        public virtual int? TenantId { get; set; }

        public virtual string Text { get; set; }
    }
}
