namespace SE.Neo.WebAPI.Models.Shared
{
    public class SkillsByCategoryModel
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategorySkillId { get; set; }
    }
}
