using System.Text.Json.Serialization;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Yenko.Users;

public class UserCommands
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<UserCommands> _logger;
    private static readonly JsonSerializerOptions s_writeOptions = new()
    {
        WriteIndented = true
    };

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

        IAsyncEnumerable<User?> users = client.GetFromJsonAsAsyncEnumerable<User>("users");

        await foreach (var user in users)
        {
            Console.WriteLine(JsonSerializer.Serialize(user, s_writeOptions) ?? string.Empty);
        }

        _logger.LogInformation("GetAllUsers Command END");

        return 0;
    }

    [Command("get", Description = "Get a user by id")]
    public async Task<int> GetUser([Option()] string id)
    {
        _logger.LogInformation("GetUser Command START");

        var client = _httpClientFactory.CreateClient("usersapi");

        var user = await client.GetFromJsonAsync<User>($"users/{id}");

        Console.WriteLine(JsonSerializer.Serialize(user, s_writeOptions) ?? string.Empty);

        _logger.LogInformation("GetUser Command END");

        return 0;
    }

    [Command("create", Description = "Create a new user")]
    public async Task<int> CreateUser([Argument] User userRequest)
    {
        _logger.LogInformation("CreateUser Command START");

        UserValidator validator = new();
        ValidationResult results = validator.Validate(userRequest);
        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");
            }

            return 1;
        }

        var client = _httpClientFactory.CreateClient("usersapi");

        var response = await client.PostAsJsonAsync("users", userRequest);

        if (response.IsSuccessStatusCode)
        {
            var createdUser = await response.Content.ReadFromJsonAsync<User>();

            Console.WriteLine(JsonSerializer.Serialize(createdUser, s_writeOptions) ?? string.Empty);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        _logger.LogInformation("CreateUser Command END");

        return 0;
    }

    [Command("update", Description = "Update an existing user")]
    public async Task<int> UpdateUser([Argument] User userRequest)
    {
        _logger.LogInformation("UpdateUser Command START");

        UserValidator validator = new();
        ValidationResult results = validator.Validate(userRequest);
        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");
            }

            return 1;
        }

        var client = _httpClientFactory.CreateClient("usersapi");

        var response = await client.PutAsJsonAsync($"users/{userRequest.Id}", userRequest);

        if (response.IsSuccessStatusCode)
        {
            var updatedUser = await response.Content.ReadFromJsonAsync<User>();

            Console.WriteLine(JsonSerializer.Serialize(updatedUser, s_writeOptions) ?? string.Empty);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        _logger.LogInformation("UpdateUser Command END");

        return 0;
    }

    [Command("delete", Description = "Delete an existing user")]
    public async Task<int> DeleteUser([Option()] int id)
    {
        _logger.LogInformation("DeleteUser Command START");

        var client = _httpClientFactory.CreateClient("usersapi");

        var response = await client.DeleteAsync($"users/{id}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"User {id} deleted");
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        _logger.LogInformation("DeleteUser Command END");

        return 0;
    }
}