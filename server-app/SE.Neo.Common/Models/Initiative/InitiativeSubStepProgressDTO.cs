namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeSubStepProgressDTO
    {

        public int SubStepId { get; set; }

        public bool IsChecked { get; set; }

        public int StepId { get; set; }

        public int InitiativeId { get; set; }

        public int CurrentStep { get; set; }
    }
}
