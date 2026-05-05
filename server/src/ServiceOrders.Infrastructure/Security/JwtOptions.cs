namespace ServiceOrders.Infrastructure.Security;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = "ServiceOrders";
    public string Key { get; init; } = "a9F#kL2!xP0@qW7$Zr8&bN5*Yt3^M1sD%uH6(J)vC4E+oGfXzR";
    public int ExpirationTime { get; init; } = 60;
}
