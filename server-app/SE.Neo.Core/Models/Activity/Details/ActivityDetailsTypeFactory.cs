using SE.Neo.Common.Attributes;

namespace SE.Neo.Core.Models.Activity.Details
{
    public static class ActivityDetailsTypeFactory
    {
        public static Type GetActivityDetailsType(this Enums.ActivityType activityType)
        {
            switch (activityType)
            {
                case Enums.ActivityType.NavMenuItemClick:
                    return typeof(NavMenuItemActivityDetails);
                case Enums.ActivityType.SearchApply:
                    return typeof(SearchActivityDetails);
                case Enums.ActivityType.FilterApply:
                    return typeof(FilterInfoActivityDetails);
                case Enums.ActivityType.CompanyProfileView:
                case Enums.ActivityType.CompanyFollow:
                case Enums.ActivityType.ContactProviderClick:
                case Enums.ActivityType.ContactProviderNevermindButtonClick:
                case Enums.ActivityType.ContactProviderConfirmButtonClick:
                    return typeof(CompanyInfoActivityDetails);
                case Enums.ActivityType.EventDetailsView:
                    return typeof(EventInfoActivityDetails);
                case Enums.ActivityType.EventAttendingButtonClick:
                    return typeof(EventAttendingInfoActivityDetails);
                case Enums.ActivityType.FirstDashboardClick:
                    return typeof(FirstClickInfoActivityDetails);
                case Enums.ActivityType.UserFollow:
                    return typeof(FollowerInfoActivityDetails);
                case Enums.ActivityType.ForumFollow:
                case Enums.ActivityType.ForumSave:
                case Enums.ActivityType.ForumView:
                    return typeof(ForumInfoActivityDetails);
                case Enums.ActivityType.LinkClick:
                    return typeof(LinkInfoActivityDetails);
                case Enums.ActivityType.ProjectSave:
                case Enums.ActivityType.ProjectView:
                    return typeof(ProjectInfoActivityDetails);
                case Enums.ActivityType.ToolClick:
                    return typeof(ToolInfoActivityDetails);
                case Enums.ActivityType.UserProfileView:
                    return typeof(UserInfoActivityDetails);
                case Enums.ActivityType.NewDiscussionClick:
                case Enums.ActivityType.ViewMapClick:
                case Enums.ActivityType.SignupClick:
                    return typeof(SignupActivityDetails);
                case Enums.ActivityType.ConnectWithNEOClick:
                case Enums.ActivityType.NewProjectClick:
                case Enums.ActivityType.LinkButtonClick:
                case Enums.ActivityType.InitiativeCreateClick:
                    return typeof(EmptyActivityDetails);
                case Enums.ActivityType.ForumMessageResponse:
                case Enums.ActivityType.ForumMessageLike:
                case Enums.ActivityType.MessageDetailsView:
                    return typeof(MessageInfoActivityDetails);
                case Enums.ActivityType.PrivateLearnClick:
                case Enums.ActivityType.LearnView:
                    return typeof(ActicleInfoActivityDetails);
                case Enums.ActivityType.TechnologiesSolutionsClick:
                    return typeof(TechnologiesSolutionsActivityDetails);
                case Enums.ActivityType.ViewAllClick:
                    return typeof(DashboardViewAllActivityDetails);
                case Enums.ActivityType.ProjectResourceClick:
                    return typeof(ProjectResourceActivityDetails);
                case Enums.ActivityType.AnnouncementButtonClick:
                    return typeof(AnnouncementButtonClickDetails);
                case Enums.ActivityType.EventRegistration:
                    return typeof(EventRegistrationActivityDetails);
                case Enums.ActivityType.SolutionDetailsView:
                    return typeof(SolutionInfoActivityDetails);
                case Enums.ActivityType.SolutionTypes:
                    return typeof(SolutionTypeInfoActivityDetails);
                case Enums.ActivityType.InitiativeDetailsView:
                case Enums.ActivityType.InitiativeModuleViewAllClick:
                    return typeof(InitiativeInfoActivityDetails);
                case Enums.ActivityType.InitiativeSubstepClick:
                    return typeof(InitiativeProgressActivityDetails);
                case Enums.ActivityType.InitiativesButtonClick:
                    return typeof(InitiativeActionsActivityDetails);
                case Enums.ActivityType.SaveContentFromAttachContentPopUp:
                    return typeof(SaveContentFromAttachInitiativePopupDetails);
                default:
                    throw new CustomException($"Can't process the activity details. Activity type {activityType} is not supported.");
            }
        }
    }
}
