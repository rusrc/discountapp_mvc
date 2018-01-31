namespace Discountapp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        MapJsonCoord = c.String(),
                        Information = c.String(),
                        Description = c.String(),
                        WorkTimeBegin = c.Time(precision: 7),
                        WorkTimeEnd = c.Time(precision: 7),
                        WorkTimeSaturdayBegin = c.Time(precision: 7),
                        WorkTimeSaturdayEnd = c.Time(precision: 7),
                        WorkTimeSundayBegin = c.Time(precision: 7),
                        WorkTimeSundayEnd = c.Time(precision: 7),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.RealEstate", t => t.AddressId)
                .Index(t => t.AddressId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Alias = c.String(),
                        MapJsonCoord = c.String(),
                        ActiveStatus = c.Int(nullable: false),
                        NameMultiLangJson = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RealEstate",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        MerchantTypeId = c.Long(),
                        UserId = c.Long(nullable: false),
                        MerchantCategoryId = c.Long(nullable: false),
                        LogoFolder = c.String(),
                        ModerationPassed = c.Boolean(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.MerchantCategory", t => t.MerchantCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.MerchantType", t => t.MerchantTypeId)
                .Index(t => t.CompanyId)
                .Index(t => t.MerchantTypeId)
                .Index(t => t.UserId)
                .Index(t => t.MerchantCategoryId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Name = c.String(),
                        LogoFolder = c.String(),
                        HotLineNumber = c.String(),
                        WebSiteLink = c.String(),
                        Description = c.String(),
                        ImageFolder = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Like",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        PromotionItemId = c.Long(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PromotionItemId })
                .ForeignKey("dbo.PromotionItem", t => t.PromotionItemId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PromotionItemId);
            
            CreateTable(
                "dbo.PromotionItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryId = c.Long(nullable: false),
                        PromotionId = c.Long(nullable: false),
                        Name = c.String(),
                        BeginPrice = c.Double(nullable: false),
                        PromotionalPrice = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        DislikeCount = c.Int(nullable: false),
                        ImageFolder = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ParentId = c.Long(),
                        NameMultiLangJson = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Promotion",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Begin = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        SubscriptionNotifierIsActive = c.Boolean(nullable: false),
                        NameMultiLangJson = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MerchantCategory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NameMultiLangJson = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MerchantType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NameMultiLangJson = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MobileUser",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        DeviceImei = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotionRealEstate",
                c => new
                    {
                        Promotion_Id = c.Long(nullable: false),
                        RealEstate_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Promotion_Id, t.RealEstate_Id })
                .ForeignKey("dbo.Promotion", t => t.Promotion_Id, cascadeDelete: true)
                .ForeignKey("dbo.RealEstate", t => t.RealEstate_Id, cascadeDelete: true)
                .Index(t => t.Promotion_Id)
                .Index(t => t.RealEstate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.MobileUser", "UserId", "dbo.User");
            DropForeignKey("dbo.Address", "AddressId", "dbo.RealEstate");
            DropForeignKey("dbo.RealEstate", "MerchantTypeId", "dbo.MerchantType");
            DropForeignKey("dbo.RealEstate", "MerchantCategoryId", "dbo.MerchantCategory");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.RealEstate", "UserId", "dbo.User");
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.Like", "UserId", "dbo.User");
            DropForeignKey("dbo.Promotion", "UserId", "dbo.User");
            DropForeignKey("dbo.PromotionRealEstate", "RealEstate_Id", "dbo.RealEstate");
            DropForeignKey("dbo.PromotionRealEstate", "Promotion_Id", "dbo.Promotion");
            DropForeignKey("dbo.PromotionItem", "PromotionId", "dbo.Promotion");
            DropForeignKey("dbo.Like", "PromotionItemId", "dbo.PromotionItem");
            DropForeignKey("dbo.PromotionItem", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Category", "ParentId", "dbo.Category");
            DropForeignKey("dbo.Company", "UserId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.RealEstate", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Address", "CityId", "dbo.City");
            DropIndex("dbo.PromotionRealEstate", new[] { "RealEstate_Id" });
            DropIndex("dbo.PromotionRealEstate", new[] { "Promotion_Id" });
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.MobileUser", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserLogin", new[] { "UserId" });
            DropIndex("dbo.Promotion", new[] { "UserId" });
            DropIndex("dbo.Category", new[] { "ParentId" });
            DropIndex("dbo.PromotionItem", new[] { "PromotionId" });
            DropIndex("dbo.PromotionItem", new[] { "CategoryId" });
            DropIndex("dbo.Like", new[] { "PromotionItemId" });
            DropIndex("dbo.Like", new[] { "UserId" });
            DropIndex("dbo.UserClaim", new[] { "UserId" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.Company", new[] { "UserId" });
            DropIndex("dbo.RealEstate", new[] { "MerchantCategoryId" });
            DropIndex("dbo.RealEstate", new[] { "UserId" });
            DropIndex("dbo.RealEstate", new[] { "MerchantTypeId" });
            DropIndex("dbo.RealEstate", new[] { "CompanyId" });
            DropIndex("dbo.Address", new[] { "CityId" });
            DropIndex("dbo.Address", new[] { "AddressId" });
            DropTable("dbo.PromotionRealEstate");
            DropTable("dbo.Subscription");
            DropTable("dbo.Role");
            DropTable("dbo.MobileUser");
            DropTable("dbo.MerchantType");
            DropTable("dbo.MerchantCategory");
            DropTable("dbo.UserRole");
            DropTable("dbo.UserLogin");
            DropTable("dbo.Promotion");
            DropTable("dbo.Category");
            DropTable("dbo.PromotionItem");
            DropTable("dbo.Like");
            DropTable("dbo.UserClaim");
            DropTable("dbo.User");
            DropTable("dbo.Company");
            DropTable("dbo.RealEstate");
            DropTable("dbo.City");
            DropTable("dbo.Address");
        }
    }
}
