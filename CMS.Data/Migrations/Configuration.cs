using CMS.Domain.Tenant;
using CMS.Domain.User;

namespace CMS.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CMS.Data.EntityFramework.CmsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CMS.Data.EntityFramework.CmsDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.IsTest = true;

            context.Tenants.AddOrUpdate(
                    new TenantEntity
                    {
                        Id = Guid.Parse("68e80c26-69b7-4697-8f40-09a632da07bc"),
                        TenancyName = "TKM",
                        DisplayName = "TKM",
                        IsActive = true
                    },
                    new TenantEntity
                    {
                        Id = Guid.Parse("7c7f548b-7ad2-4186-8ce2-20479f0b1b15"),
                        TenancyName = "bcsint",
                        DisplayName = "bcsint",
                        IsActive = true
                    }
                );

            context.Users.AddOrUpdate(
                    new UserEntity
                    {
                        Id = Guid.Parse("56f554e5-8d57-4d3a-8e4f-22fa099a108d"),
                        UserName = "carl",
                        FirstName = "carl",
                        LastName = "dai",
                        Email = "dc0106@126.com",
                        IsActive = true,
                        Password = "AGNYLBZ7MwHB+t1yvuI2mIej3UF/lYyyfEX0OAPUR+d52DA3+HlA5hftzv5T51O/7g==",
                        TenantId = Guid.Parse("68e80c26-69b7-4697-8f40-09a632da07bc")
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("1094dc1d-58ab-4968-925f-76583a08159a"),
                        UserName = "admin",
                        FirstName = "dai",
                        LastName = "change",
                        Email = "280141563@qq.com",
                        IsActive = true,
                        Password = "ABIWKasL8u8gAj8ZVRQJuRlBFu1oxO6Tk2eYm6iRezcL4F75SF1ns9IH0Gd0+K79kw==",
                        TenantId = Guid.Parse("7c7f548b-7ad2-4186-8ce2-20479f0b1b15")
                    }
                );
        }
    }
}
