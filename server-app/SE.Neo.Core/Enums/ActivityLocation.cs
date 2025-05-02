using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum ActivityLocation : int
    {
        [Description("Admin page")]
        Admin = 1,

        [Description("Admit users")]
        AdmitUsers,

        [Description("Review user")]
        ReviewUser,

        [Description("User management")]
        UserManagement,

        [Description("Add user")]
        AddUser,

        [Description("Edit user")]
        EditUser,

        [Description("Company management")]
        CompanyManagement,

        [Description("Add company")]
        AddCompany,

        [Description("Edit company")]
        EditCompany,

        [Description("Group management")]
        GroupManagement,

        [Description("Add group")]
        AddGroup,

        [Description("Edit group")]
        EditGroup,

        [Description("Learn page")]
        Learn,

        [Description("Learn details")]
        LearnDetails,

        [Description("Events page")]
        Events,

        [Description("Event details")]
        EventDetails,

        [Description("Add event")]
        AddEvent,

        [Description("Forums page")]
        Forums,

        [Description("Forum details")] ForumDetails,

        [Description("Add forum")]
        AddForum,

        [Description("Tool management")]
        ToolManagement,

        [Description("Tools page")]
        Tools,

        [Description("Add tool")]
        AddTool,

        [Description("Edit tool")]
        EditTool,

        [Description("View tool")]
        ViewTool,

        [Description("Email alert settings")]
        EmailAlertSettings,

        [Description("Announcement management")]
        AnnouncementManagement,

        [Description("Add announcement")]
        AddAnnouncement,

        [Description("Edit announcement")]
        EditAnnouncement,

        [Description("Dashboard page")]
        Dashboard,

        [Description("Project catalog")]
        ProjectCatalog,

        [Description("Project details")]
        ProjectDetails,

        [Description("Project library")]
        ProjectLibrary,

        [Description("Add project")]
        AddProject,

        [Description("Edit project")]
        EditProject,

        [Description("Community page")]
        Community,

        [Description("Search result page")]
        SearchResult,

        [Description("Notifications page")]
        Notifications,

        [Description("Messages page")]
        Messages,

        [Description("Add message")]
        AddMessage,

        [Description("View message")]
        ViewMessage,

        [Description("Saved content page")]
        SavedContent,

        [Description("Topic page")]
        Topic,

        [Description("View user profile")]
        ViewUserProfile,

        [Description("Edit user profile")]
        EditUserProfile,

        [Description("View company profile")]
        ViewCompanyProfile,

        [Description("Account settings")]
        AccountSettings,

        [Description("Registration Page")]
        RegistrationPage,

        [Description("Solution Details Page")]
        SolutionDetails,

        [Description("Solution Types Page")]
        SolutionTypes,

        [Description("Manage")]
        ManageProfile,

        [Description("View Initiative")]
        ViewInitiative,

        [Description("Create Initiative")]
        CreateInitiative,

        [Description("Initiative Manage Module Page")]
        InitiativeManageModulePage
    }
}
