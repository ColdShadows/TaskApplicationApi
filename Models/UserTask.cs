namespace TaskApplicationApi.Models
{
    public class UserTask
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string ParentId { get; set; }

        public string Status { get; set; }

        public float PercentageComplete { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletionDate { get; set; }
    }
}
