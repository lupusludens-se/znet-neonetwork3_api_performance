namespace SE.Neo.Core.Constants
{
    public static class CoreErrorMessages
    {
        public const string UnexpectedError = "Unexpected error occurred. Please try again later or contact us.";
        public const string RoleNotSupported = "This Role is not supported currently.";
        public const string ErrorOnSaving = "Error occurred on saving data.";
        public const string ErrorOnReading = "Error occurred on reading data.";
        public const string ErrorOnRemoving = "Error occurred on removing data.";
        public const string UserIsNotActive = "User account is inactive.";
        public const string FollowerExists = "User is already followed.";
        public const string ErrorOnCMS = "Error occurred with response from CMS.";
        public const string SaveNotificationTypeError = "Error occured during grouping notification with type {0}. The type is not supported.";
        public const string NotificationDetailsAreNotValid = "Notification details {0} are not valid for notification type {1}";
        public const string EntityNotFound = "Requested entity not found.";
        public const string InvalidFileSignature = "Invalid file type.";
        public const string ForumNotFoundOrDeleted = "Forum {0} not found or it is deleted.";
        public const string UserPendingNotFound = "User Pending does not exist.";
        public const string TimeZoneNotFound = "Time Zone {0} not found.";
        public const string SavedContentAlreadySaved = "{0} with corresponding id has already been saved for this user.";
        public const string SavedContentAlreadyDeleted = "{0} with corresponding id does not exist for the user.";
        public const string ProhibitedFollowYourself = "You can't follow yourself.";
        public const string ErrorOnInitiativeCreation = "This initiative can't be created as you have already created the maximum of 3.";
        public const string InitiativeFileMaxLimit = "This file can't be uploaded as you have already uploaded the maximum of 5 files.";
        public const string OnlyLogoAccepted = "Only image files are accepted.";
        public const string InitiativeReplaceNoAccess = "This file can't be replaced as you are not a owner";
    }
}
