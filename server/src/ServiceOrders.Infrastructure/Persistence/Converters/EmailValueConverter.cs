using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceOrders.Domain.ValueObjects;

namespace ServiceOrders.Infrastructure.Persistence.Converters;

public sealed class EmailValueConverter : ValueConverter<Email, string>
{
    public EmailValueConverter()
        : base(
            email => email.Value,
            value => Email.Create(value))
    { }
}
