namespace CMS.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class InitalCreate : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.Actions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActionCode = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 512),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Guid(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ActionEntity_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.Actions", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Actions", "DeleterUserId", c => c.Guid());
            AddColumn("dbo.Actions", "DeletionTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Actions", "DeletionTime");
            DropColumn("dbo.Actions", "DeleterUserId");
            DropColumn("dbo.Actions", "IsDeleted");
            AlterTableAnnotations(
                "dbo.Actions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActionCode = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 512),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Guid(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ActionEntity_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}
