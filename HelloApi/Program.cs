var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Register all endpoint groups
app.MapUsersEndpoints();
// UsersEndpoints.MapUsersEndpoints(app);

app.MapGet("/", () => "Welcome to API");
app.Run();