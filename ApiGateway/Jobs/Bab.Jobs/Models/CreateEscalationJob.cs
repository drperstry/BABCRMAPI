namespace Bab.Jobs.Models
{
    public class CreateEscalationJob
    {
        public int RecurrenceFrequency { get; set; }
        public DateTime Time { get; set; }

        public string EntityId { get; set; }
    }
}
