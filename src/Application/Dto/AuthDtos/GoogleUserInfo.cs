using System.Text.Json.Serialization;

namespace DevKid.src.Application.Dto.AuthDtos
{
    public class GoogleUserInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("verified_email")]
        public bool Verified_Email { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("given_name")]
        public string Given_Name { get; set; }
        [JsonPropertyName("family_name")]
        public string Family_Name { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }
}
