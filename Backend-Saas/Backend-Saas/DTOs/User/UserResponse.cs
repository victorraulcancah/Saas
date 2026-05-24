namespace Backend_Saas.DTOs.User;

public record UserResponse(Guid Id, string Email, string? FirstName, string? LastName, bool IsActive);
