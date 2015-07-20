﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.TestBase.SampleApplication.People;

namespace Abp.TestBase.SampleApplication.ContacLists
{
    [Table("ContactLists")]
    public class ContactList: Entity, IMustHaveTenant<int>
    {
        public virtual int TenantId { get; set; }

        public virtual string Name { get; set; }

        [ForeignKey("ContactListId")]
        public virtual ICollection<Person> People { get; set; }
    }
}
