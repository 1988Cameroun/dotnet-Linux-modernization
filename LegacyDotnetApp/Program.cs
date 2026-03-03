var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    // Fix 1: Use cross-platform path handling instead of hardcoded Windows path
    var logPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "logs", "app.log"
    );

    // Fix 2: Use Environment.NewLine instead of hardcoded CRLF
    var message = $"App started{Environment.NewLine}";

    // Fix 3: Use cross-platform environment variable
    var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    return new {
        status = "running",
        logPath = logPath,
        lineEnding = message.Contains("\r\n") ? "Windows CRLF" : "Unix LF",
        userHome = userHome,
        os = System.Runtime.InteropServices.RuntimeInformation.OSDescription
    };
});

app.Run();
