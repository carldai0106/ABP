﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.TestBase.SampleApplication.ContacLists;

namespace Abp.TestBase.SampleApplication.People
{
    [Table("People")]
    public class Person : Entity, IDeletionAudited<long>
    {
        public const int MaxNameLength = 64;

        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [ForeignKey("ContactListId")]
        public virtual ContactList ContactList { get; set; }

        public virtual int ContactListId { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual long? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
