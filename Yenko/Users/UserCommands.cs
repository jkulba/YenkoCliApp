using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Yenko.Users;

public class UserCommands
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<UserCommands> _logger;

    public UserCommands(IHttpClientFactory httpClientFactory, ILogger<UserCommands> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [Command("list", Description = "List all users")]
    public async Task<int> GetAllUsers()
    {
        _logger.LogInformation("GetAllUsers Command START");

        var client = _httpClientFactory.CreateClient("usersapi");

        IAsyncEnumerable<UserResponse?> users = client.GetFromJsonAsAsyncEnumerable<UserResponse>("users");

        await foreach (var user in users)
        {
            Console.WriteLine(JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }) ?? string.Empty);
        }

        _logger.LogInformation("GetAllUsers Command END");

        return 0;
    }

    [Command("get", Description = "Get user by id")]
    public async Task<int> GetUser([Option()] string id)
    {
        _logger.LogInformation("GetUser Command START");

        var client = _httpClientFactory.CreateClient("usersapi");

        var user = await client.GetFromJsonAsync<UserResponse>($"users/{id}");

        Console.WriteLine(JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }) ?? string.Empty);

        _logger.LogInformation("GetUser Command END");

        return 0;
    }

    [Command("create", Description = "Create new user")]
    public async Task<int> CreateUser([Argument] UserCommands.UserRequest userRequest)
    {
        _logger.LogInformation("CreateUser Command START");

        var client = _httpClientFactory.CreateClient("usersapi");

        var response = await client.PostAsJsonAsync("users", userRequest);

        if (response.IsSuccessStatusCode)
        {
            var createdUser = await response.Content.ReadFromJsonAsync<UserResponse>();

            Console.WriteLine(JsonSerializer.Serialize(createdUser, new JsonSerializerOptions { WriteIndented = true }) ?? string.Empty);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        _logger.LogInformation("CreateUser Command END");

        return 0;
    }

    public class UserRequest
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("email")] public string? Email { get; set; }
        [JsonPropertyName("username")] public string? UserName { get; set; }
        [JsonPropertyName("password")] public string? Password { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("address")] public Address? Address { get; set; }
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

}