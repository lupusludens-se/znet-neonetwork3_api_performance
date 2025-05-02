namespace SE.Neo.Core.Enums
{/// <summary>
/// Search Parameters for Users
/// </summary>
    public enum CommunityUserSearchyByFieldsEnum
    {
        FullName, FirstName, LastName, Company, UserProfile, Categories, Category, Name, JobTitle, Country, State
    }
    /// <summary>
    /// Search Parameters for Company
    /// </summary>
    public enum CommunityCompanySearchyByFieldsEnum
    {
        Name, Industry, Categories, Category
    }
    /// <summary>
    /// Order display
    /// </summary>
    public enum CommunitySearchOrderEnum
    {
        user_FullName = 0, user_FirstName = 1, user_LastName = 2, company_Name = 3, user_Company_Name = 4, user_UserProfile_Categories = 5, company_Industry_Name = 6, company_Categories = 7, user_Country_Name = 8, user_UserProfile_State_Name = 9, user_UserProfile_JobTitle = 10
    }
}
