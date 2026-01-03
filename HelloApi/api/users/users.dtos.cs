// ============= Data Models =============
namespace users_dtos
{

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Additional fields you might want:
        // public string PhoneNumber { get; set; } = string.Empty;
        // public string Address { get; set; } = string.Empty;
        // public bool IsActive { get; set; } = true;
        // public string Role { get; set; } = "User";
    }

    public class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
    }

    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
    }

    public class UpdateEmailDto
    {
        public string Email { get; set; } = string.Empty;

    }
}