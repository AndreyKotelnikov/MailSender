namespace CodeFirstDbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationships : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipient_RecipientsList",
                c => new
                    {
                        ListId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ListId, t.RecipientId })
                .ForeignKey("dbo.RecipientsListEntities", t => t.ListId, cascadeDelete: true)
                .ForeignKey("dbo.RecipientEntities", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.ListId)
                .Index(t => t.RecipientId);
            
            AddColumn("dbo.RecipientsListEntities", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.MailMessageEntities", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.RecipientEntities", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.SchedulerTaskEntities", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.SenderEntities", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ServerEntities", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.RecipientsListEntities", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.MailMessageEntities", "Subject", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.MailMessageEntities", "Body", c => c.String(maxLength: 3000));
            AlterColumn("dbo.RecipientEntities", "ConnectAdress", c => c.String(maxLength: 255));
            AlterColumn("dbo.RecipientEntities", "Description", c => c.String(maxLength: 2000));
            AlterColumn("dbo.RecipientEntities", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SchedulerTaskEntities", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SenderEntities", "ConnectAdress", c => c.String(maxLength: 255));
            AlterColumn("dbo.SenderEntities", "Description", c => c.String(maxLength: 2000));
            AlterColumn("dbo.SenderEntities", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ServerEntities", "UserName", c => c.String(maxLength: 255));
            AlterColumn("dbo.ServerEntities", "ConnectAdress", c => c.String(maxLength: 255));
            AlterColumn("dbo.ServerEntities", "Description", c => c.String(maxLength: 2000));
            AlterColumn("dbo.ServerEntities", "Name", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.SchedulerTaskEntities", "MailMessageId");
            CreateIndex("dbo.SchedulerTaskEntities", "SenderId");
            CreateIndex("dbo.SchedulerTaskEntities", "ListId");
            CreateIndex("dbo.SchedulerTaskEntities", "ServerId");
            AddForeignKey("dbo.SchedulerTaskEntities", "MailMessageId", "dbo.MailMessageEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchedulerTaskEntities", "SenderId", "dbo.SenderEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchedulerTaskEntities", "ServerId", "dbo.ServerEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchedulerTaskEntities", "ListId", "dbo.RecipientsListEntities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SchedulerTaskEntities", "ListId", "dbo.RecipientsListEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "ServerId", "dbo.ServerEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "SenderId", "dbo.SenderEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "MailMessageId", "dbo.MailMessageEntities");
            DropForeignKey("dbo.Recipient_RecipientsList", "RecipientId", "dbo.RecipientEntities");
            DropForeignKey("dbo.Recipient_RecipientsList", "ListId", "dbo.RecipientsListEntities");
            DropIndex("dbo.Recipient_RecipientsList", new[] { "RecipientId" });
            DropIndex("dbo.Recipient_RecipientsList", new[] { "ListId" });
            DropIndex("dbo.SchedulerTaskEntities", new[] { "ServerId" });
            DropIndex("dbo.SchedulerTaskEntities", new[] { "ListId" });
            DropIndex("dbo.SchedulerTaskEntities", new[] { "SenderId" });
            DropIndex("dbo.SchedulerTaskEntities", new[] { "MailMessageId" });
            AlterColumn("dbo.ServerEntities", "Name", c => c.String());
            AlterColumn("dbo.ServerEntities", "Description", c => c.String());
            AlterColumn("dbo.ServerEntities", "ConnectAdress", c => c.String());
            AlterColumn("dbo.ServerEntities", "UserName", c => c.String());
            AlterColumn("dbo.SenderEntities", "Name", c => c.String());
            AlterColumn("dbo.SenderEntities", "Description", c => c.String());
            AlterColumn("dbo.SenderEntities", "ConnectAdress", c => c.String());
            AlterColumn("dbo.SchedulerTaskEntities", "Name", c => c.String());
            AlterColumn("dbo.RecipientEntities", "Name", c => c.String());
            AlterColumn("dbo.RecipientEntities", "Description", c => c.String());
            AlterColumn("dbo.RecipientEntities", "ConnectAdress", c => c.String());
            AlterColumn("dbo.MailMessageEntities", "Body", c => c.String());
            AlterColumn("dbo.MailMessageEntities", "Subject", c => c.String());
            AlterColumn("dbo.RecipientsListEntities", "Name", c => c.String());
            DropColumn("dbo.ServerEntities", "RowVersion");
            DropColumn("dbo.SenderEntities", "RowVersion");
            DropColumn("dbo.SchedulerTaskEntities", "RowVersion");
            DropColumn("dbo.RecipientEntities", "RowVersion");
            DropColumn("dbo.MailMessageEntities", "RowVersion");
            DropColumn("dbo.RecipientsListEntities", "RowVersion");
            DropTable("dbo.Recipient_RecipientsList");
        }
    }
}
