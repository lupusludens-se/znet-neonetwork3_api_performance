namespace SE.Neo.Core.Entities.Interfaces
{
    public interface ITimeStamp
    {
        public int? CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
