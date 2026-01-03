var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World API!");
app.MapGet("/api/hello", () => "Hello from API!");
app.MapGet("/api/greet/{name}", (string name) => $"Hello, {name}!");

app.Run();