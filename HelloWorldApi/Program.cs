var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Simple GET endpoint that returns "Hello World!"
app.MapGet("/", () => "Hello World!");

app.Run();