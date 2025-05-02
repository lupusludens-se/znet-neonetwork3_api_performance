namespace SE.Neo.Common.Models.Project
{
    public class BaseProjectDetailsDTO
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public ProjectDTO Project { get; set; }
    }
}
