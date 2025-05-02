using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum DashboardClickElementActionType
    {
        [Description("Messages View")]
        MessagesView = 1,
        [Description("Pinned Tools Customize")]
        PinnedToolsCustomize,
        [Description("Pinned Tool View")]
        PinnedToolView,
        [Description("Pinned Tool Add")]
        PinnedToolAdd,
        [Description("Learn View All")]
        LearnViewAll,
        [Description("Learn View")]
        LearnView,
        [Description("Learn Save")]
        LearnSave,
        [Description("Learn Unsave")]
        LearnUnsave,
        [Description("Learn Tag Click")]
        LearnTagClick,
        [Description("Events View All")]
        EventsViewAll,
        [Description("Event View")]
        EventView,
        [Description("Announcement Button Click")]
        AnnouncementButtonClick,
        [Description("Suggestion Hide")]
        SuggestionHide,
        [Description("Suggestion Skip")]
        SuggestionSkip,
        [Description("Suggestion Take")]
        SuggestionTake,
        [Description("Project Catalog Browse")]
        ProjectCatalogBrowse,
        [Description("About Solutions Click")]
        AboutSolutionsClick,
        [Description("About Technologies Click")]
        AboutTechnologiesClick,
        [Description("Forums View All")]
        ForumsViewAll,
        [Description("Forum View")]
        ForumView,
        [Description("Forum Category Click")]
        ForumCategoryClick,
        [Description("Forum Region Click")]
        ForumRegionClick,
        [Description("Forum Save")]
        ForumSave,
        [Description("Forum Unsave")]
        ForumUnsave,
        [Description("Forum Pin")]
        ForumPin,
        [Description("Forum Unpin")]
        ForumUnpin,
        [Description("Forum Owner Follow")]
        ForumOwnerFollow,
        [Description("Forum Owner Unfollow")]
        ForumOwnerUnfollow,
        [Description("Forum Owner View")]
        ForumOwnerView,
        [Description("Forum Like")]
        ForumLike,
        [Description("Forum UnLike")]
        ForumUnLike,
        [Description("Forum Comment")]
        ForumComment,
        [Description("Forum Delete")]
        ForumDelete,
        [Description("Project View")]
        ProjectView,
        [Description("Project Save")]
        ProjectSave,
        [Description("Project Unsave")]
        ProjectUnsave,
        [Description("Project Tag Click")]
        ProjectTagClick,
        [Description("Company View")]
        CompanyView,
        [Description("Companies View All")]
        CompaniesViewAll,
        [Description("Companies Browse")]
        CompaniesBrowse,
        [Description("Project Discoverability Item View")]
        ProjectDiscoverabilityItemView,
        [Description("Initiative Create Click")]
        InitiativeCreateClick,
        [Description("Initiative View")]
        InitiativeVeiw,

    }
}
