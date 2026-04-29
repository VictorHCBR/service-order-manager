using ServiceOrders.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace ServiceOrders.Domain.ValueObjects;

public sealed partial record Email
{
    private static readonly Regex EmailRegex = RegexRule();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex RegexRule();
    
    public string Value { get; private init; }

    private Email() => Value = string.Empty;

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email é obrigatório.");
        
        var normalized = value.Trim().ToLowerInvariant();
        if (!EmailRegex.IsMatch(normalized))
            throw new DomainException("Email inválido.");

        return new Email(normalized);
    }
}
