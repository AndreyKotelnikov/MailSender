namespace CodeFirstDbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CanceledCascadeOnDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchedulerTaskEntities", "ListId", "dbo.RecipientsListEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "MailMessageId", "dbo.MailMessageEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "SenderId", "dbo.SenderEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "ServerId", "dbo.ServerEntities");
            AddForeignKey("dbo.SchedulerTaskEntities", "ListId", "dbo.RecipientsListEntities", "Id");
            AddForeignKey("dbo.SchedulerTaskEntities", "MailMessageId", "dbo.MailMessageEntities", "Id");
            AddForeignKey("dbo.SchedulerTaskEntities", "SenderId", "dbo.SenderEntities", "Id");
            AddForeignKey("dbo.SchedulerTaskEntities", "ServerId", "dbo.ServerEntities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SchedulerTaskEntities", "ServerId", "dbo.ServerEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "SenderId", "dbo.SenderEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "MailMessageId", "dbo.MailMessageEntities");
            DropForeignKey("dbo.SchedulerTaskEntities", "ListId", "dbo.RecipientsListEntities");
            AddForeignKey("dbo.SchedulerTaskEntities", "ServerId", "dbo.ServerEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchedulerTaskEntities", "SenderId", "dbo.SenderEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchedulerTaskEntities", "MailMessageId", "dbo.MailMessageEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchedulerTaskEntities", "ListId", "dbo.RecipientsListEntities", "Id", cascadeDelete: true);
        }
    }
}
