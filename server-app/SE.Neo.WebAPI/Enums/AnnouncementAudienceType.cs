
using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>  
    /// Enum representing different types of announcement audiences.  
    /// </summary>  
    public enum AnnouncementAudienceType
    {
        /// <summary>  
        /// Audience type for corporations.  
        /// </summary>  
        Corporation = RoleType.Corporation,

        /// <summary>  
        /// Audience type for solution providers.  
        /// </summary>  
        SolutionProvider = RoleType.SolutionProvider,

        /// <summary>  
        /// Audience type for internal users.  
        /// </summary>  
        Internal = RoleType.Internal,

        /// <summary>  
        /// Audience type for system owner.  
        /// </summary>
        SystemOwner = RoleType.SystemOwner,

        /// <summary>  
        /// Audience type for all users.  
        /// </summary>  
        All = RoleType.All
    }
}
