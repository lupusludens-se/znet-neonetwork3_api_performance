namespace SE.Neo.Common.Models.CMS
{
    public class BaseTaxonomyDTO
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public string Description { get; set; }
    }
}
