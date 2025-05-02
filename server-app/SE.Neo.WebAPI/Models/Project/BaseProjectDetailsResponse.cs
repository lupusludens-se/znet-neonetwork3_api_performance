namespace SE.Neo.WebAPI.Models.Project
{
    public class BaseProjectDetailsResponse
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public ProjectResponse Project { get; set; }
    }
}
