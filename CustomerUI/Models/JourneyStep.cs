namespace CustomerUI.Models
{
    public class JourneyStep
    {
        public int JourneyStepId { get; set; }
        public int CustomerId { get; set; }
        public string StepName { get; set; } = null!;
        public DateTime StepDate { get; set; }
        public int StepOrder { get; set; }
    }
}
