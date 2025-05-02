using SE.Neo.Common.Enums;
using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discussion")]
    public class Discussion : BaseIdEntity
    {
        [Column("Discussion_Id")]
        public override int Id { get; set; }

        [MaxLength(250)]
        [Column("Discussion_Subject")]
        public string Subject { get; set; }

        [Column("Discussion_Type")]
        public DiscussionType Type { get; set; }

        [Column("Discussion_Is_Pinned")]
        public bool IsPinned { get; set; }

        [Column("Discussion_Is_Deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        ///  To capture the datetime for discussion updates such as Add/Edit/Likes/Responses/Replies.
        /// </summary>
        [Column("Discussion_Updated_Ts")]
        public DateTime? DiscussionUpdatedOn { get; set; }

        [Column("Discussion_Source_Type")]
        [ForeignKey("Source")]
        public DiscussionSourceType? SourceTypeId { get; set; }

        public DiscussionSource? Source { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        public Project? Project { get; set; }

        public ICollection<DiscussionUser> DiscussionUsers { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<DiscussionCategory> DiscussionCategories { get; set; }

        public ICollection<DiscussionRegion> DiscussionRegions { get; set; }

        public ICollection<DiscussionFollower> DiscussionFollowers { get; set; }

        public ICollection<DiscussionSaved> DiscussionSaved { get; set; }
    }
}