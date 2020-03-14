using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Data Seeding 
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "This is home page of eShopSolution" },
                new AppConfig() { Key = "HomeKeyword", Value = "This is key word of eShopSolution" },
                new AppConfig() { Key = "HomeDescription", Value = "This is description of eShopSolution" }
                );


            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "vi-US", Name = "English", IsDefault = false }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Enums.Status.Active,

                },
                new Category()
                {
                    Id = 2,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 2,
                    Status = Enums.Status.Active,

                });

            modelBuilder.Entity<CategoryTranslation>().HasData(
                    new CategoryTranslation() {Id=1, CategoryId = 1, Name = "Áo nam", LanguageId = "vi-VN", SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                    new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men Shirt", LanguageId = "vi-US", SeoAlias = "men-shirt", SeoDescription = "The shirt products for men", SeoTitle = "The shirt products for men" },
                     new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = "vi-VN", SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang nữ" },
                    new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women Shirt", LanguageId = "vi-US", SeoAlias = "women-shirt", SeoDescription = "The shirt products for women", SeoTitle = "The shirt products for women" }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 100000,
                    Price = 200000,
                    Stock = 0,
                    ViewCount = 0,  
                });
            modelBuilder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id=1,
                     ProductId = 1,
                     Name = "Áo sơ mi nam trắng Việt Tiến",
                     LanguageId = "vi-VN",
                     SeoAlias = "ao-so-mi-nam-trang-Viet-Tien",
                     SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                     SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                     Details = "Áo sơ mi nam trắng Việt Tiến",
                     Description = "Áo sơ mi nam trắng Việt Tiến"
                 },
                    new ProductTranslation()
                    {
                        Id=2,
                        ProductId = 1,
                        Name = "Viet Tien Men T Shirt",
                        LanguageId = "vi-US",
                        SeoAlias = "Viet-Tien-men-T-shirt",
                        SeoDescription = "Viet-Tien-men-T-shirt",
                        SeoTitle = "Viet-Tien-men-T-shirt",
                        Details = "Viet-Tien-men-T-shirt",
                        Description = "Viet-Tien-men-T-shirt"
                    });
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { CategoryId = 1, ProductId = 1 }
                );

            // https://stackoverflow.com/questions/50785009/how-to-seed-an-admin-user-in-ef-core-2-1-0
            // any guid    
            var roleId = new Guid("07FE94AE-F960-442B-BADC-3417826F6FD5");
            var adminId = new Guid("194E1ED3-45B3-42FB-81BE-44DB403BB56B");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description="Adminstrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "an@dhpd.edu.com",
                NormalizedEmail = "an@dhpd.edu.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "abc1234$"),
                SecurityStamp = string.Empty,
                FirstName="AN",
                LastName="Pham",
                Dob=new DateTime(2020,03,03)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
            //
        }



    }
}
