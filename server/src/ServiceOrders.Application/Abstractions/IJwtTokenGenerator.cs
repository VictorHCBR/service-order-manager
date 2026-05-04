using ServiceOrders.Domain.Entities.Users;

namespace ServiceOrders.Application.Abstractions;

public sealed record JwtToken(string AccessToken, DateTimeOffset ExpiresAt);

public interface IJwtTokenGenerator
{
    JwtToken Generate(User user);
}
