var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    // Windows-style path assumption — will break on Linux
    var logPath = @"C:\Logs\app.log";
    
    // Windows line endings assumption
    var message = "App started\r\n";
    
    // Windows environment variable assumption
    var userProfile = Environment.GetEnvironmentVariable("USERPROFILE") 
                      ?? "USERPROFILE not found — not a Windows environment";

    return new {
        status = "running",
        logPath = logPath,
        lineEnding = message.Contains("\r\n") ? "Windows CRLF" : "Unix LF",
        userProfile = userProfile,
        os = System.Runtime.InteropServices.RuntimeInformation.OSDescription
    };
});

app.Run();
