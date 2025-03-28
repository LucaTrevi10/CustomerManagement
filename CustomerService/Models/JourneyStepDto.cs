namespace CustomerService.Models
{
    public class JourneyStepDto
    {
        public int JourneyStepId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public DateTime StepDate { get; set; }
        public int StepOrder { get; set; }
    }
}
