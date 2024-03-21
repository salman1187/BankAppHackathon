﻿namespace BankApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccNo = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Pin = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateOfOpening = c.DateTime(nullable: false),
                        Balance = c.Double(nullable: false),
                        PrivilegeType = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.AccNo);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransID = c.Int(nullable: false),
                        AccNo = c.Int(nullable: false),
                        TransType = c.String(),
                        TransDate = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.TransID, t.AccNo });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
            DropTable("dbo.Accounts");
        }
    }
}
