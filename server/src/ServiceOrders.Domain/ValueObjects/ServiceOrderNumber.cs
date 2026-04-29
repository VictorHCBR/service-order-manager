using ServiceOrders.Domain.Exceptions;

namespace ServiceOrders.Domain.ValueObjects;

public sealed record ServiceOrderNumber
{
    public string Value { get; private init; }

    private ServiceOrderNumber() => Value = string.Empty;

    private ServiceOrderNumber(string value) => Value = value;

    public static ServiceOrderNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Número da OS é obrigatório.");

        var normalized = value.Trim().ToUpperInvariant();
        if(normalized.Length < 6 || normalized.Length > 30)
            throw new DomainException("Número da OS deve conter entre 6 e 30 caracteres.");

        return new ServiceOrderNumber(normalized);
    }

    public static ServiceOrderNumber Generate(DateTimeOffset now, int sequence)
        => new($"OS-{now:yyyyMMdd}-{sequence:D5}");

    public override string ToString() => Value;
}
