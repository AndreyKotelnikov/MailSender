namespace CodeFirstDbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteNavigetionProperties : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RecipientsLists", newName: "RecipientsListEntities");
            RenameTable(name: "dbo.Recipients", newName: "RecipientEntities");
            RenameTable(name: "dbo.MailMessages", newName: "MailMessageEntities");
            RenameTable(name: "dbo.Senders", newName: "SenderEntities");
            RenameTable(name: "dbo.Servers", newName: "ServerEntities");
            DropForeignKey("dbo.RecipientRecipientsLists", "Recipient_Id", "dbo.Recipients");
            DropForeignKey("dbo.RecipientRecipientsLists", "RecipientsList_Id", "dbo.RecipientsLists");
            DropForeignKey("dbo.SchedulerTasks", "List_Id", "dbo.RecipientsLists");
            DropForeignKey("dbo.SchedulerTasks", "MailMessage_Id", "dbo.MailMessages");
            DropForeignKey("dbo.SchedulerTasks", "Sender_Id", "dbo.Senders");
            DropForeignKey("dbo.SchedulerTasks", "Server_Id", "dbo.Servers");
            DropIndex("dbo.SchedulerTasks", new[] { "List_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "MailMessage_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "Sender_Id" });
            DropIndex("dbo.SchedulerTasks", new[] { "Server_Id" });
            DropIndex("dbo.RecipientRecipientsLists", new[] { "Recipient_Id" });
            DropIndex("dbo.RecipientRecipientsLists", new[] { "RecipientsList_Id" });
            CreateTable(
                "dbo.SchedulerTaskEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanedTime = c.DateTime(nullable: false),
                        MailMessageId = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        ListId = c.Int(nullable: false),
                        ServerId = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.SchedulerTasks");
            DropTable("dbo.RecipientRecipientsLists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RecipientRecipientsLists",
                c => new
                    {
                        Recipient_Id = c.Int(nullable: false),
                        RecipientsList_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipient_Id, t.RecipientsList_Id });
            
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
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.SchedulerTaskEntities");
            CreateIndex("dbo.RecipientRecipientsLists", "RecipientsList_Id");
            CreateIndex("dbo.RecipientRecipientsLists", "Recipient_Id");
            CreateIndex("dbo.SchedulerTasks", "Server_Id");
            CreateIndex("dbo.SchedulerTasks", "Sender_Id");
            CreateIndex("dbo.SchedulerTasks", "MailMessage_Id");
            CreateIndex("dbo.SchedulerTasks", "List_Id");
            AddForeignKey("dbo.SchedulerTasks", "Server_Id", "dbo.Servers", "Id");
            AddForeignKey("dbo.SchedulerTasks", "Sender_Id", "dbo.Senders", "Id");
            AddForeignKey("dbo.SchedulerTasks", "MailMessage_Id", "dbo.MailMessages", "Id");
            AddForeignKey("dbo.SchedulerTasks", "List_Id", "dbo.RecipientsLists", "Id");
            AddForeignKey("dbo.RecipientRecipientsLists", "RecipientsList_Id", "dbo.RecipientsLists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RecipientRecipientsLists", "Recipient_Id", "dbo.Recipients", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.ServerEntities", newName: "Servers");
            RenameTable(name: "dbo.SenderEntities", newName: "Senders");
            RenameTable(name: "dbo.MailMessageEntities", newName: "MailMessages");
            RenameTable(name: "dbo.RecipientEntities", newName: "Recipients");
            RenameTable(name: "dbo.RecipientsListEntities", newName: "RecipientsLists");
        }
    }
}
