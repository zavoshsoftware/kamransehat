namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCourseDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CourseDetailId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseDetails", t => t.CourseDetailId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseDetailId);
            
            CreateTable(
                "dbo.CourseDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        PresentDate = c.DateTime(nullable: false),
                        Teachers = c.String(),
                        CourseId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        Summery = c.String(),
                        Body = c.String(),
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
            
            DropColumn("dbo.Users", "Token");
            DropColumn("dbo.Users", "RemainCredit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RemainCredit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Users", "Token", c => c.String());
            DropForeignKey("dbo.UserCourseDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCourseDetails", "CourseDetailId", "dbo.CourseDetails");
            DropForeignKey("dbo.CourseDetails", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseDetails", new[] { "CourseId" });
            DropIndex("dbo.UserCourseDetails", new[] { "CourseDetailId" });
            DropIndex("dbo.UserCourseDetails", new[] { "UserId" });
            DropTable("dbo.Courses");
            DropTable("dbo.CourseDetails");
            DropTable("dbo.UserCourseDetails");
        }
    }
}
