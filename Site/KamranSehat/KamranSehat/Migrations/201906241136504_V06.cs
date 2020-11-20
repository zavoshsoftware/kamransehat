namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V06 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 15),
                        Title = c.String(nullable: false, maxLength: 256),
                        PageTitle = c.String(nullable: false, maxLength: 500),
                        PageDescription = c.String(maxLength: 1000),
                        ImageUrl = c.String(maxLength: 500),
                        Summery = c.String(),
                        Body = c.String(storeType: "ntext"),
                        IsInHome = c.Boolean(nullable: false),
                        BookUrl = c.String(),
                        Size = c.String(),
                        WorsthRate = c.Int(nullable: false),
                        BestRate = c.Int(nullable: false),
                        AverageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RateCount = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Radios",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 15),
                        Title = c.String(nullable: false, maxLength: 256),
                        PageTitle = c.String(nullable: false, maxLength: 500),
                        PageDescription = c.String(maxLength: 1000),
                        ImageUrl = c.String(maxLength: 500),
                        Summery = c.String(),
                        Body = c.String(storeType: "ntext"),
                        IsInHome = c.Boolean(nullable: false),
                        RedioUrl = c.String(),
                        Duration = c.String(),
                        Size = c.String(),
                        WorsthRate = c.Int(nullable: false),
                        BestRate = c.Int(nullable: false),
                        AverageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RateCount = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Rates", "Book_Id", c => c.Guid());
            AddColumn("dbo.Rates", "Radio_Id", c => c.Guid());
            CreateIndex("dbo.Rates", "Book_Id");
            CreateIndex("dbo.Rates", "Radio_Id");
            AddForeignKey("dbo.Rates", "Book_Id", "dbo.Books", "Id");
            AddForeignKey("dbo.Rates", "Radio_Id", "dbo.Radios", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rates", "Radio_Id", "dbo.Radios");
            DropForeignKey("dbo.Rates", "Book_Id", "dbo.Books");
            DropIndex("dbo.Rates", new[] { "Radio_Id" });
            DropIndex("dbo.Rates", new[] { "Book_Id" });
            DropColumn("dbo.Rates", "Radio_Id");
            DropColumn("dbo.Rates", "Book_Id");
            DropTable("dbo.Radios");
            DropTable("dbo.Books");
        }
    }
}
