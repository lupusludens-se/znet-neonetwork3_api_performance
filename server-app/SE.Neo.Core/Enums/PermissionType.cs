using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum PermissionType
    {
        [Description("Admin All")]
        AdminAll = 1,
        [Description("Announcement Management")]
        AnnouncementManagement = 2,
        [Description("Company Management")]
        CompanyManagement = 3,
        [Description("Data Sync")]
        DataSync = 4,
        [Description("Event Management")]
        EventManagement = 5,
        [Description("Messages All")]
        MessagesAll = 6,
        [Description("Project Catalog View")]
        ProjectCatalogView = 7,
        [Description("Project Management All")]
        ProjectManagementAll = 8,
        [Description("Project Management Own")]
        ProjectManagementOwn = 9,
        [Description("Send Quote")]
        SendQuote = 10,
        [Description("Tool Management")]
        ToolManagement = 11,
        [Description("Forum Management")]
        ForumManagement = 12,
        [Description("User Access Management")]
        UserAccessManagement = 13,
        [Description("User Profile Edit")]
        UserProfileEdit = 14,
        [Description("Email Alert Management")]
        EmailAlertManagement = 15,
        [Description("Project Private Details View")]
        ProjectPrivateDetailsView = 16,
        [Description("Manage Company Users")]
        ManageCompanyUsers = 17,
        [Description("Manage Own Company")]
        ManageOwnCompany = 18,
        [Description("Manage Company Projects")]
        ManageCompanyProjects = 19,
        [Description("View Company Messages")]
        ViewCompanyMessages = 20,
        [Description("Initiative Management Own")]
        InitiativeManageOwn = 21,
        [Description("Initiative Management All")]
        InitiativeManageAll = 22,
        [Description("Tier Management")]
        TierManagement = 23
    }
}
