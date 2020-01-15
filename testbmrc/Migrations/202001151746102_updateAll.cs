namespace testbmrc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAll : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessLists",
                c => new
                    {
                        AccessListID = c.Int(nullable: false, identity: true),
                        AccessName = c.String(),
                        AccessValue = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        IsGranted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AccessListID);
            
            CreateTable(
                "dbo.CallCategories",
                c => new
                    {
                        CallCategoryID = c.Int(nullable: false, identity: true),
                        CallCategoryName = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CallCategoryID);
            
            CreateTable(
                "dbo.CallHistories",
                c => new
                    {
                        CallHistoryID = c.Int(nullable: false, identity: true),
                        CallerName = c.String(),
                        CallerPhone = c.String(),
                        CallerEmail = c.String(),
                        CallerAddress = c.String(),
                        CountryID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        CompanyVsStaffID = c.Int(nullable: false),
                        CallTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        CallCategoryID = c.Int(nullable: false),
                        Subject = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CallHistoryID)
                .ForeignKey("dbo.CallCategories", t => t.CallCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.CompanyVsStaffs", t => t.CompanyVsStaffID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.CompanyVsStaffID)
                .Index(t => t.CallCategoryID);
            
            CreateTable(
                "dbo.CompanyVsStaffs",
                c => new
                    {
                        CompanyVsStaffID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        LoginName = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        CompanyStaffImage = c.String(),
                        CompanyStaffImageByte = c.Binary(),
                        RoleID = c.Int(),
                        UserRightPermissionID = c.Int(),
                        Status = c.Int(nullable: false),
                        IsItSuperAdmin = c.Boolean(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyVsStaffID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .ForeignKey("dbo.UserRightPermissions", t => t.UserRightPermissionID)
                .Index(t => t.CompanyID)
                .Index(t => t.RoleID)
                .Index(t => t.UserRightPermissionID);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        PostCode = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        CountryID = c.Int(nullable: false),
                        Details = c.String(),
                        CompanyLogo = c.String(),
                        CompanyLogoByte = c.Binary(),
                        Tag = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.Countries", t => t.CountryID, cascadeDelete: true)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.UserRightPermissions",
                c => new
                    {
                        UserRightPermissionID = c.Int(nullable: false, identity: true),
                        UserRightPermissionName = c.String(),
                        UserRightPermissionDescription = c.String(),
                        UserRightPermissionDetails = c.String(),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserRightPermissionID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        LoginName = c.String(),
                        Password = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        RoleID = c.Int(),
                        UserRightPermissionID = c.Int(),
                        EmployeeOwnImageBytes = c.Binary(),
                        EmployeeOwnImageBytesPaths = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .ForeignKey("dbo.UserRightPermissions", t => t.UserRightPermissionID)
                .Index(t => t.RoleID)
                .Index(t => t.UserRightPermissionID);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        SchoolID = c.Int(nullable: false, identity: true),
                        SchoolName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        CourseStructure = c.Int(nullable: false),
                        ApplicationPA = c.String(),
                        AcceptedPA = c.String(),
                        SuccessRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsGraduateEntryAvailable = c.Boolean(nullable: false),
                        FoundationOrAccessCoursesAvailable = c.Boolean(nullable: false),
                        IntercalatedBSc = c.Int(nullable: false),
                        GCSESubjectsRequired = c.String(),
                        A_LevelSubjectsRequired = c.String(),
                        A_LevelGradesRequired = c.String(),
                        ScottishHighersSubjectsRequired = c.String(),
                        ScottishHigherGradesRequired = c.String(),
                        ScottishAdvancedHighersSubjectsRequired = c.String(),
                        ScottishAdvancedHighersGradesRequired = c.String(),
                        IBSubjectsRequired = c.String(),
                        IBGradesRequired = c.String(),
                        UCATRequired = c.Boolean(nullable: false),
                        HowisUCATused = c.String(),
                        BMATRequired = c.Boolean(nullable: false),
                        HowisBMATUsed = c.String(),
                        InterviewStyle = c.Int(nullable: false),
                        Fees_EUStudents = c.String(),
                        Fees_NonEUStudents = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SchoolID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CallHistories", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "UserRightPermissionID", "dbo.UserRightPermissions");
            DropForeignKey("dbo.Employees", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.CallHistories", "CompanyVsStaffID", "dbo.CompanyVsStaffs");
            DropForeignKey("dbo.CompanyVsStaffs", "UserRightPermissionID", "dbo.UserRightPermissions");
            DropForeignKey("dbo.CompanyVsStaffs", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.CompanyVsStaffs", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Company", "CountryID", "dbo.Countries");
            DropForeignKey("dbo.CallHistories", "CallCategoryID", "dbo.CallCategories");
            DropIndex("dbo.Employees", new[] { "UserRightPermissionID" });
            DropIndex("dbo.Employees", new[] { "RoleID" });
            DropIndex("dbo.Company", new[] { "CountryID" });
            DropIndex("dbo.CompanyVsStaffs", new[] { "UserRightPermissionID" });
            DropIndex("dbo.CompanyVsStaffs", new[] { "RoleID" });
            DropIndex("dbo.CompanyVsStaffs", new[] { "CompanyID" });
            DropIndex("dbo.CallHistories", new[] { "CallCategoryID" });
            DropIndex("dbo.CallHistories", new[] { "CompanyVsStaffID" });
            DropIndex("dbo.CallHistories", new[] { "EmployeeID" });
            DropTable("dbo.Schools");
            DropTable("dbo.Employees");
            DropTable("dbo.UserRightPermissions");
            DropTable("dbo.Roles");
            DropTable("dbo.Countries");
            DropTable("dbo.Company");
            DropTable("dbo.CompanyVsStaffs");
            DropTable("dbo.CallHistories");
            DropTable("dbo.CallCategories");
            DropTable("dbo.AccessLists");
        }
    }
}
