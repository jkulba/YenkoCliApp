using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Cocona.Command.Binder;

var builder = CoconaApp.CreateBuilder();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Logging.AddFilter("System.Net.Http", LogLevel.Warning);

builder.Services.AddSingleton<ICoconaValueConverter, JsonValueConverter>();

builder.Services.AddHttpClient("usersapi", c =>
{
    c.BaseAddress = new Uri("http://localhost:3000/");
});

builder.Host.UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostContext.Configuration)
);

var app = builder.Build();

app.AddCommands<UserCommands>();

app.Run();