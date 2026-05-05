using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceOrders.Domain.ValueObjects;

namespace ServiceOrders.Infrastructure.Persistence.Converters;

public sealed class ServiceOrderNumberValueConverter : ValueConverter<ServiceOrderNumber, string>
{
    public ServiceOrderNumberValueConverter()
        : base(
            number => number.Value,
            value => ServiceOrderNumber.Create(value))
    { }
}
