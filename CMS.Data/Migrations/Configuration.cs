using System;
using System.Data.Entity.Migrations;
using CMS.Data.EntityFramework;
using CMS.Domain.Tenant;
using CMS.Domain.User;
using EntityFramework.DynamicFilters;

namespace CMS.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CmsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CmsDbContext context)
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

            context.DisableAllFilters();
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
                    TenancyName = "Default",
                    DisplayName = "Default",
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
                    FirstName = "carl",
                    LastName = "dai",
                    Email = "280141563@qq.com",
                    IsActive = true,
                    Password = "ABIWKasL8u8gAj8ZVRQJuRlBFu1oxO6Tk2eYm6iRezcL4F75SF1ns9IH0Gd0+K79kw==",
                    TenantId = Guid.Parse("7c7f548b-7ad2-4186-8ce2-20479f0b1b15")
                }
                );
        }
    }
}