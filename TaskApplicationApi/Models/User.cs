using Newtonsoft.Json;

namespace TaskApplicationApi.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("usersubject")]
        public string UserSubject { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferencesId { get; set; }
    }
}
