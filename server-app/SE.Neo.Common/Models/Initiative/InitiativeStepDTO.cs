namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeStepDTO
    {
        public int StepId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<InitiativeSubStepDTO> SubSteps { get; set; }
    }
}
