// UsersEndpoints.cs
using users_dtos;

public static class UsersEndpoints
{
    private static int GetNextId() => users.Any() ? users.Max(u => u.Id) + 1 : 1;
    private static List<User> users = new()
    {
        new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Age = 25 },
        new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Age = 30 }
    };
    public static void MapUsersEndpoints(this WebApplication app)
    {

        var usersGroup = app.MapGroup("/api/users")
            .WithTags("Users");

        // GET /api/users
        usersGroup.MapGet("/", All_users).WithName("Users");

        // GET /api/users/{id}
        usersGroup.MapGet("/{id}", Get_user_by_id);

        // POST /api/users
        usersGroup.MapPost("/", Create_User);

        // PUT /api/users/{id}
        usersGroup.MapPut("/{id}", Update_user);

        // DELETE /api/users/{id}
        usersGroup.MapDelete("/{id}", Delete_user);
    }
    public static List<User> All_users()
    {
        return users;
    }

    private static IResult Get_user_by_id(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
            return Results.Ok(user);
        return Results.NotFound($"User with ID {id} not found");

    }
    private static IResult Create_User(CreateUserDto newUser)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(newUser.FirstName) ||
            string.IsNullOrWhiteSpace(newUser.LastName) ||
            string.IsNullOrWhiteSpace(newUser.Email))
            return Results.BadRequest("All fields are required");

        if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
            return Results.BadRequest("Email already exists");

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
    }
    private static IResult Update_user(int id, UpdateUserDto updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Results.NotFound($"User with ID {id} not found");

        // Update logic...
        return Results.Ok(user);
    }
    private static IResult Delete_user(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Results.NotFound($"User with ID {id} not found");

        users.Remove(user);
        return Results.Ok($"User with ID {id} deleted");
    }
}