using Newtonsoft.Json;

namespace PersonApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SocialAccount
    {
        public long Id { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public string Address { get; set; }
    }
}
