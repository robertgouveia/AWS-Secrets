using Amazon;
using Amazon.Runtime;
using Secrets.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hooking project up to AWS Secrets
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;
builder.Configuration.AddSecretsManager(
    region: RegionEndpoint.EUNorth1, 
    configurator: options =>
{
    // If it matches the below then load them
    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
    options.KeyGenerator = (_, s) => s.Replace($"{env}_{appName}_", string.Empty) // remove prefix
        .Replace("__", ":"); // Swapping __ for :
    options.PollingInterval = TimeSpan.FromDays(30); // How often it will collect the new variables
});

// Binds to class to the value of the section
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();