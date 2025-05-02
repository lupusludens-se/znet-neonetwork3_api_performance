namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeStepResponse
    {
        public int StepId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<InitiativeSubStepResponse> SubSteps { get; set; }
    }
}
