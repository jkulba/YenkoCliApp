using System.Text.Json.Serialization;

namespace Yenko.Users;

public record User
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("username")] public string? UserName { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("address")] public Address? Address { get; set; }

}

public record Address
{
    [JsonPropertyName("number")] public int Number { get; set; }
    [JsonPropertyName("street")] public string? Street { get; set; }
    [JsonPropertyName("city")] public string? City { get; set; }
    [JsonPropertyName("state")] public string? State { get; set; }
    [JsonPropertyName("zipcode")] public string? Zipcode { get; set; }
    [JsonPropertyName("geolocation")] public Geolocation? Geolocation { get; set; }
}

public record Geolocation
{
    [JsonPropertyName("latitude")] public string? Latitude { get; set; }
    [JsonPropertyName("longitude")] public string? Longitude { get; set; }
}
