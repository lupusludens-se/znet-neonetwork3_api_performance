namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeAndProgressDetailsDTO : BaseInitiativeDTO
    {
        public IEnumerable<InitiativeStepDTO> Steps { get; set; }

        public IEnumerable<InitiativeSubStepProgressDTO> SubStepsProgress { get; set; }

    }
}
