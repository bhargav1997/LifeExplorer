namespace WagWander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FantacyPawCare : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inquiries",
                c => new
                    {
                        InquiryId = c.Int(nullable: false, identity: true),
                        PetName = c.String(),
                        PetId = c.Int(nullable: false),
                        Username = c.String(),
                        InquiryText = c.String(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.InquiryId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.PetId)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        PetId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Species = c.String(),
                        Breed = c.String(),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PetId);
            
            CreateTable(
                "dbo.MediaItems",
                c => new
                    {
                        MediaItemID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Genre = c.String(),
                    })
                .PrimaryKey(t => t.MediaItemID);
            
            CreateTable(
                "dbo.UserMediaItems",
                c => new
                    {
                        UserMediaItemID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        MediaItemID = c.Int(nullable: false),
                        Rating = c.Int(),
                        Review = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.UserMediaItemID)
                .ForeignKey("dbo.MediaItems", t => t.MediaItemID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.MediaItemID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Bio = c.String(),
                        FavoriteGenre = c.String(),
                        JoinDate = c.DateTime(nullable: false),
                        Location = c.String(),
                        UserHasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMediaItems", "UserID", "dbo.Users");
            DropForeignKey("dbo.Inquiries", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.UserMediaItems", "MediaItemID", "dbo.MediaItems");
            DropForeignKey("dbo.Inquiries", "PetId", "dbo.Pets");
            DropIndex("dbo.UserMediaItems", new[] { "MediaItemID" });
            DropIndex("dbo.UserMediaItems", new[] { "UserID" });
            DropIndex("dbo.Inquiries", new[] { "User_UserID" });
            DropIndex("dbo.Inquiries", new[] { "PetId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserMediaItems");
            DropTable("dbo.MediaItems");
            DropTable("dbo.Pets");
            DropTable("dbo.Inquiries");
        }
    }
}
