using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum ActivityType
    {
        [Description("Navigation menu item click")]
        NavMenuItemClick = 1,

        [Description("Search filter apply")]
        SearchApply,

        [Description("Specific filter apply")]
        FilterApply,

        [Description("Company profile open")]
        CompanyProfileView,

        [Description("Follow a company")]
        CompanyFollow,

        [Description("Contact Provider button click")]
        ContactProviderClick,

        [Description("Nevermind button click during contacting provider")]
        ContactProviderNevermindButtonClick,

        [Description("Confirm button click during contacting provider")]
        ContactProviderConfirmButtonClick,

        [Description("Event details open")]
        EventDetailsView,

        [Description("Attending yes/no button click")]
        EventAttendingButtonClick,

        [Description("First click on an element on dashboard")]
        FirstDashboardClick,

        [Description("Start following a user")]
        UserFollow,

        [Description("Start following a forum")]
        ForumFollow,

        [Description("Add a forum to saved items")]
        ForumSave,

        [Description("Forum open")]
        ForumView,

        [Description("Click on a link")]
        LinkClick,

        [Description("Add a project to saved items")]
        ProjectSave,

        [Description("Project details open")]
        ProjectView,

        [Description("Click on a tool")]
        ToolClick,

        [Description("User profile open")]
        UserProfileView,

        [Description("Start a Discussion button click")]
        NewDiscussionClick,

        [Description("View Map button click")]
        ViewMapClick,

        [Description("Connect with NEO button click")]
        ConnectWithNEOClick,

        [Description("New Project button click")]
        NewProjectClick,

        [Description("Response on a forum message")]
        ForumMessageResponse,

        [Description("Forum message like button click")]
        ForumMessageLike,

        [Description("Article open")]
        LearnView,

        [Description("About Technologies/Solutions button click")]
        TechnologiesSolutionsClick,

        [Description("View all button click")]
        ViewAllClick,

        [Description("Project resource click")]
        ProjectResourceClick,

        [Description("Link button click")]
        LinkButtonClick,

        [Description("Announcement Button Click")]
        AnnouncementButtonClick,

        [Description("Signup Click")]
        SignupClick,

        [Description("Event Registration")]
        EventRegistration,

        [Description("Private Article Click")]
        PrivateLearnClick,

        [Description("Solution Details View")]
        SolutionDetailsView,

        [Description("Solution Types")]
        SolutionTypes,

        [Description("Initiative Create Click")]
        InitiativeCreateClick,

        [Description("Initiative Details View")]
        InitiativeDetailsView,

        [Description("Initiative Sub Step Click")]
        InitiativeSubstepClick,

        [Description("Initiative Module View All Click")]
        InitiativeModuleViewAllClick,

        [Description("Initiatives Button Click")]
        InitiativesButtonClick,

        [Description("Message Details View")]
        MessageDetailsView,

        [Description("Save Content To An Initiative From Attach Pop up")]
        SaveContentFromAttachContentPopUp
    }
}
