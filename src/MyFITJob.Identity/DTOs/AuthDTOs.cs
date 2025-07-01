using System.ComponentModel.DataAnnotations;

namespace MyFITJob.Identity.DTOs;

public record LoginRequest(
    [Required] string Username,
    [Required] string Password
);

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    string UserId,
    string Username,
    string Email,
    List<string> Roles
);

public record RefreshTokenRequest(
    [Required] string RefreshToken
);

public record RegisterRequest(
    [Required] string Username,
    [Required][EmailAddress] string Email,
    [Required] string Password,
    [Required] string FirstName,
    [Required] string LastName
);

public record UserDto(
    string Id,
    string Username,
    string Email,
    List<string> Roles
);

public record UpdateUserDto(
    [Required][EmailAddress] string Email,
    [Required] string FirstName,
    [Required] string LastName,
    bool IsActive
); 