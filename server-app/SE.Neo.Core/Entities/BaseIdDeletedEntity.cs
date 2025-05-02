namespace SE.Neo.Core.Entities
{
    public class BaseCMSEntity : BaseIdEntity
    {
        public virtual bool IsDeleted { get; set; }
        public virtual string Name { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Description { get; set; }
    }
}
