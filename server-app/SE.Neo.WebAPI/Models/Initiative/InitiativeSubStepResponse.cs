namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeSubStepResponse
    {

        public int SubStepId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int Order { get; set; }
        public bool IsChecked { get; set; }
    }
}
