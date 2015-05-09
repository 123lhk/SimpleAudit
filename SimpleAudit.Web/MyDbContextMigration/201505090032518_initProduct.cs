namespace SimpleAudit.Web.MyDbContextMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TargetId = c.Guid(nullable: false),
                        TargetType = c.String(),
                        ModifiedByUserId = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        PropertyName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Made = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
            DropTable("dbo.AuditLogs");
        }
    }
}
