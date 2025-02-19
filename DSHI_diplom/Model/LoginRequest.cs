using System.Text.Json.Serialization;

namespace DSHI_diplom.Model
{
    public class LoginRequest
    {
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; } 
    }
}
