using users_dtos;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory list to store users (for learning purposes)
// In real apps, use a database
var users = new List<User>
{
    new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Age = 25 },
    new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Age = 30 }
};

// Helper to generate unique ID
int GetNextId() => users.Any() ? users.Max(u => u.Id) + 1 : 1;

// ============= GET Endpoints =============

// Get all users
app.MapGet("/api/users", () =>
{
    return Results.Ok(users);
});

// Get user by ID
app.MapGet("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound($"User with ID {id} not found");

    return Results.Ok(user);
});

// Search users by name
app.MapGet("/api/users/search", (string? name) =>
{
    if (string.IsNullOrWhiteSpace(name))
        return Results.BadRequest("Please provide a search term");

    var filteredUsers = users.Where(u =>
        u.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
        u.LastName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
        u.Email.Contains(name, StringComparison.OrdinalIgnoreCase)
    ).ToList();

    return Results.Ok(filteredUsers);
});

// ============= POST Endpoints =============

// Create new user
app.MapPost("/api/users", (CreateUserDto newUser) =>
{
    // Basic validation
    if (string.IsNullOrWhiteSpace(newUser.FirstName))
        return Results.BadRequest("First name is required");

    if (string.IsNullOrWhiteSpace(newUser.LastName))
        return Results.BadRequest("Last name is required");

    if (string.IsNullOrWhiteSpace(newUser.Email))
        return Results.BadRequest("Email is required");

    // Check if email already exists
    if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
        return Results.BadRequest("Email already exists");

    // Create user
    var user = new User
    {
        Id = GetNextId(),
        FirstName = newUser.FirstName.Trim(),
        LastName = newUser.LastName.Trim(),
        Email = newUser.Email.Trim().ToLower(),
        Age = newUser.Age,
        CreatedAt = DateTime.UtcNow
    };

    users.Add(user);

    return Results.Created($"/api/users/{user.Id}", user);
});

// ============= PUT Endpoints =============

// Update user
app.MapPut("/api/users/{id}", (int id, UpdateUserDto updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound($"User with ID {id} not found");

    // Update fields if provided
    if (!string.IsNullOrWhiteSpace(updatedUser.FirstName))
        user.FirstName = updatedUser.FirstName.Trim();

    if (!string.IsNullOrWhiteSpace(updatedUser.LastName))
        user.LastName = updatedUser.LastName.Trim();

    if (!string.IsNullOrWhiteSpace(updatedUser.Email))
    {
        var email = updatedUser.Email.Trim().ToLower();
        // Check if email already exists for another user
        if (users.Any(u => u.Id != id && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return Results.BadRequest("Email already exists");

        user.Email = email;
    }

    if (updatedUser.Age.HasValue)
        user.Age = updatedUser.Age.Value;

    user.UpdatedAt = DateTime.UtcNow;

    return Results.Ok(user);
});

// ============= DELETE Endpoints =============

// Delete user
app.MapDelete("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound($"User with ID {id} not found");

    users.Remove(user);
    return Results.Ok($"User with ID {id} deleted successfully");
});

// ============= PATCH Endpoints =============

// Partial update (update only email)
app.MapPatch("/api/users/{id}/email", (int id, UpdateEmailDto emailUpdate) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound($"User with ID {id} not found");

    if (string.IsNullOrWhiteSpace(emailUpdate.Email))
        return Results.BadRequest("Email is required");

    var email = emailUpdate.Email.Trim().ToLower();

    // Check if email already exists for another user
    if (users.Any(u => u.Id != id && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        return Results.BadRequest("Email already exists");

    user.Email = email;
    user.UpdatedAt = DateTime.UtcNow;

    return Results.Ok(user);
});

// ============= Health Check =============

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        Status = "Healthy",
        UserCount = users.Count,
        Timestamp = DateTime.UtcNow
    });
});

// ============= Root Endpoint =============

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Message = "User Management API",
        Endpoints = new
        {
            GetUsers = "GET /api/users",
            GetUserById = "GET /api/users/{id}",
            CreateUser = "POST /api/users",
            UpdateUser = "PUT /api/users/{id}",
            DeleteUser = "DELETE /api/users/{id}",
            SearchUsers = "GET /api/users/search?name={name}"
        }
    });
});

app.Run();

