namespace CMS.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenancyName = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Guid(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TenancyName, unique: true, name: "UNQ_Tenants_TenancyName");
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 64),
                        Password = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(nullable: false, maxLength: 128),
                        TenantId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Guid(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .Index(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "TenantId", "dbo.Tenants");
            DropIndex("dbo.Users", new[] { "TenantId" });
            DropIndex("dbo.Tenants", "UNQ_Tenants_TenancyName");
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Tenants",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
