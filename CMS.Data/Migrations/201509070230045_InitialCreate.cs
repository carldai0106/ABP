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
                "dbo.ActionModules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActionId = c.Guid(nullable: false),
                        ModuleId = c.Guid(nullable: false),
                        Status = c.Boolean(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actions", t => t.ActionId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ActionId)
                .Index(t => t.ModuleId);
            
            CreateTable(
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
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ActionEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleRights",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        ActionModuleId = c.Guid(nullable: false),
                        Status = c.Boolean(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActionModules", t => t.ActionModuleId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ActionModuleId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        ModuleCode = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 256),
                        DisplayAsMenu = c.Boolean(nullable: false),
                        DisplayAsFrontMenu = c.Boolean(nullable: false),
                        ClassName = c.String(maxLength: 128),
                        AppUrl = c.String(maxLength: 256),
                        FrontUrl = c.String(maxLength: 256),
                        Order = c.Int(nullable: false),
                        Description = c.String(maxLength: 512),
                        IsActive = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ModuleEntity_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_ModuleEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ParentId)
                .ForeignKey("dbo.Tenants", t => t.TenantId, cascadeDelete: true)
                .Index(t => t.ParentId)
                .Index(t => t.TenantId);
            
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
                .PrimaryKey(t => t.Id);
            
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
                        IsActive = c.Boolean(nullable: false),
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
                    { "DynamicFilter_UserEntity_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_UserEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tenants", t => t.TenantId, cascadeDelete: true)
                .Index(t => t.TenantId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        Status = c.Boolean(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Guid(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleCode = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 512),
                        IsActive = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
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
                    { "DynamicFilter_RoleEntity_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_RoleEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tenants", t => t.TenantId, cascadeDelete: true)
                .Index(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Roles", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.RoleRights", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Modules", "ParentId", "dbo.Modules");
            DropForeignKey("dbo.ActionModules", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.RoleRights", "ActionModuleId", "dbo.ActionModules");
            DropForeignKey("dbo.ActionModules", "ActionId", "dbo.Actions");
            DropIndex("dbo.Roles", new[] { "TenantId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "TenantId" });
            DropIndex("dbo.Modules", new[] { "TenantId" });
            DropIndex("dbo.Modules", new[] { "ParentId" });
            DropIndex("dbo.RoleRights", new[] { "ActionModuleId" });
            DropIndex("dbo.RoleRights", new[] { "RoleId" });
            DropIndex("dbo.ActionModules", new[] { "ModuleId" });
            DropIndex("dbo.ActionModules", new[] { "ActionId" });
            DropTable("dbo.Roles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RoleEntity_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_RoleEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserEntity_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_UserEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Tenants",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Modules",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ModuleEntity_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_ModuleEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RoleRights");
            DropTable("dbo.Actions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ActionEntity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ActionModules");
        }
    }
}
