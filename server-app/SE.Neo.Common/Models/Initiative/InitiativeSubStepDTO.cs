namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeSubStepDTO
    {
        public int SubStepId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public bool IsChecked { get; set; }

    }
}
