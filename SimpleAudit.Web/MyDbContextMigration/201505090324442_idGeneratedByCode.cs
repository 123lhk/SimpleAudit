namespace SimpleAudit.Web.MyDbContextMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idGeneratedByCode : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AuditLogs");
            AlterColumn("dbo.AuditLogs", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.AuditLogs", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AuditLogs");
            AlterColumn("dbo.AuditLogs", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.AuditLogs", "Id");
        }
    }
}
