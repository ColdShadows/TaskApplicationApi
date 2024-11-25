using Newtonsoft.Json;

namespace TaskApplicationApi.Models
{
    public class UserPreferences
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userid")]
        public string UserId { get; set; }

        public string PreferencesJson { get; set; }

        public int PreferencesVersion { get; set; }
    }
}
