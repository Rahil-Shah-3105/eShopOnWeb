using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

public class Address // ValueObject
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Street { get; private set; }

    public string City { get; private set; }

    public string State { get; private set; }

    public string Country { get; private set; }

    public string ZipCode { get; private set; }

    #pragma warning disable CS8618 // Required by Entity Framework
    private Address() { }

    public Address(string street, string city, string state, string country, string zipcode)
    {
		_logger.LogInformation("ENTERING: Address() method");
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipcode;
    }
}
