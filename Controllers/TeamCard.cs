using Newtonsoft.Json;

namespace TeamsTfsBridge.Model
{
    public class TeamCard
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("potentialAction")]
        public PotentialAction[] PotentialAction { get; set; }
    }

    public class PotentialAction
    {
        [JsonProperty("@context")]
        public string Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("target")]
        public string[] Target { get; set; }
    }
}
