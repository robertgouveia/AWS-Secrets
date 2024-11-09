using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Secrets.Models;

namespace Secrets.Controllers;

[ApiController]
public class ExampleController : ControllerBase
{
    // IOptionsMonitor Accounts for edits of the value
    private readonly IOptionsMonitor<DatabaseSettings> _databaseSettings;

    public ExampleController(IOptionsMonitor<DatabaseSettings> databaseSettings) => _databaseSettings = databaseSettings;

    [HttpGet("settings")]
    public IActionResult Settings() => Ok(_databaseSettings.CurrentValue);
}