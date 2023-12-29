using System.Text.Json.Serialization;

namespace Yenko.Users;

public record UserResponse
{
    [JsonPropertyName("address")] public Address? Address { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("username")] public string? UserName { get; set; }
    [JsonPropertyName("password")] public string? Password { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }

}

public record Address
{
    [JsonPropertyName("geolocation")] public Geolocation? Geolocation { get; set; }
    [JsonPropertyName("city")] public string? City { get; set; }
    [JsonPropertyName("street")] public string? Street { get; set; }
    [JsonPropertyName("number")] public int Number { get; set; }
    [JsonPropertyName("zipcode")] public string? Zipcode { get; set; }
}

public record Geolocation
{
    [JsonPropertyName("lat")] public string? Latitude { get; set; }
    [JsonPropertyName("lng")] public string? Longitude { get; set; }
}
