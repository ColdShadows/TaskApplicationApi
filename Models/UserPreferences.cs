using Newtonsoft.Json;

namespace TaskApplicationApi.Models
{
    public class UserPreferences
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userid")]
        public string UserId { get; set; }

        public string ThemeName { get; set; }
    }
}
