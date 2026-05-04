namespace ServiceOrders.Application.Auth;

public sealed record RegisterUserRequest(string Name, string Email, string Password, string[] Roles);
public sealed record LoginRequest(string Email, string Password);
public sealed record AuthResponse(Guid UserId, string Name, string Email, string[] Roles, string AccessToken, DateTimeOffset ExpiresAt);