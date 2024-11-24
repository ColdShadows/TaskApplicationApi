using Newtonsoft.Json;

namespace TaskApplicationApi.Models
{
    public class UserTask
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userid")]
        public string UserId { get; set; }

        public string Description { get; set; }

        public string ParentId { get; set; }

        public string Status { get; set; }

        public float PercentageComplete { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
