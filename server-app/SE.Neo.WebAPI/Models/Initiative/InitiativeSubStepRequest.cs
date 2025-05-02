namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeSubStepRequest
    {
        public int InitiativeId { get; set; }
        public int SubStepId { get; set; }

        public int StepId { get; set; }
        public bool IsChecked { get; set; }
        public int CurrentStep { get; set; }
    }
}
