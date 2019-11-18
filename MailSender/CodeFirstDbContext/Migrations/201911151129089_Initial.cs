namespace CodeFirstDbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecipientsLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConnectAdress = c.String(),
                        Description = c.String(),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SchedulerTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanedTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        List_Id = c.Int(),
                        MailMessage_Id = c.Int(),
                        Sender_Id = c.Int(),
                        Server_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RecipientsLists", t => t.List_Id)
                .ForeignKey("dbo.MailMessages", t => t.MailMessage_Id)
                .ForeignKey("dbo.Senders", t => t.Sender_Id)
                .ForeignKey("dbo.Servers", t => t.Server_Id)
                .Index(t => t.List_Id)
                .Index(t => t.MailMessage_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.Server_Id);
            
            CreateTable(
                "dbo.Senders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConnectAdress = c.String(),
                        Description = c.String(),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Port = c.Int(nullable: false),
                        UseSSL = c.Boolean(nullable: false),
                        UserName = c.String(),
                        ConnectAdress = c.String(),
                        Description = c.String(),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecipientRecipientsLists",
                c => new
                    {
                        Recipient_Id = c.Int(nullable: false),
                        RecipientsList_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipient_Id, t.RecipientsList_Id })
                .ForeignKey("dbo.Recipients", t => t.Recipient_Id, cascadeDelete: true)
                .ForeignKey("dbo.RecipientsLists", t => t.RecipientsList_Id, cascadeDelete: true)
                .Index(t => t.Recipient_Id)
                .Index(t => t.RecipientsList_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SchedulerTasks", "Server_Id", "dbo.Servers");
            DropForeignKey("dbo.SchedulerTasks", "Sender_Id", "dbo.Senders");
            DropForeignKey("dbo.SchedulerTasks", "MailMessage_Id", "dbo.MailMessages");
            DropForeignKey("dbo.SchedulerTasks", "List_Id", "dbo.RecipientsLists");
            DropForeignKey("dbo.RecipientRecipientsLists", "RecipientsList_Id", "dbo.RecipientsLists");
            DropForeignKey("dbo.RecipientRecipientsLists", "Recipient_Id", "dbo.Recipients");
            DropIndex("dbo.RecipientRecipientsLists", new[] { "RecipientsList_Id" });
            DropIndex("dbo.RecipientRecipientsLists", new[] { "Recipient_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "Server_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "Sender_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "MailMessage_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "List_Id" });
            DropTable("dbo.RecipientRecipientsLists");
            DropTable("dbo.Servers");
            DropTable("dbo.Senders");
            DropTable("dbo.SchedulerTasks");
            DropTable("dbo.MailMessages");
            DropTable("dbo.Recipients");
            DropTable("dbo.RecipientsLists");
        }
    }
}
