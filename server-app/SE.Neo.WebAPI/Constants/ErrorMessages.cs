
namespace SE.Neo.WebAPI.Constants
{
    public static class ErrorMessages
    {
        /// <summary>  
        /// Error message indicating lack of permission.  
        /// </summary>  
        public const string NotHavePermission = "You are not authorized to perform this action";

        /// <summary>  
        /// Error message indicating invalid model state.  
        /// </summary>  
        public const string ModelStateInvalid = "Model state invalid.";

        /// <summary>  
        /// Error message indicating missing data for field ID or empty model.  
        /// </summary>  
        public const string MissingDataForFieldIdOrModelEmpty = "Missing data for field id or model is empty.";

        /// <summary>  
        /// Error message indicating that the ID must be a positive number.  
        /// </summary>  
        public const string PositiveIdValue = "Id must be a positive number.";

        /// <summary>  
        /// Error message indicating an issue during the addition of a notification.  
        /// </summary>  
        public const string ErrorAddingNotification = "Something went wrong during adding notification. See details of exception.";

        /// <summary>  
        /// Error message indicating that categories are not allowed for corporations.  
        /// </summary>  
        public const string CategoriesNotAllowedForCorporation = "Categories are not allowed for corporation.";

        /// <summary>  
        /// Error message indicating that Offsite PPAs are not allowed for corporations.  
        /// </summary>  
        public const string OffsitePPAsNotAllowedForCorporation = "OffsitePPAs are not allowed for corporation.";

        /// <summary>  
        /// Error message indicating that categories should contain at least one element.  
        /// </summary>  
        public const string CategoriesNotEmpty = "Categories should contain at least 1 element.";

        /// <summary>  
        /// Error message indicating that the article ID must be a valid CMS ID.  
        /// </summary>  
        public const string ArticleNotValid = "ArticleId must be a valid article cms id.";

        /// <summary>  
        /// Error message indicating that there are too many characters in the About field.  
        /// </summary>  
        public const string AboutMaxLength = "There are too many characters in About Field";

        /// <summary>  
        /// Error message indicating that there are too many characters in the About Text field.  
        /// </summary>  
        public const string AboutTextMaxLength = "There are too many characters in About Text Field";

        /// <summary>  
        /// Error message indicating an issue while updating a user.  
        /// </summary>  
        public const string ErrorUpdatingUser = "Error while updating user";
    }
}
