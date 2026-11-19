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
		_logger.LogError(">>> Entering Address(street={Street},city={City},state={State},country={Country},zipcode=<redacted>)", street, city, state, country);
        Street = street;
		_logger.LogWarning("<<< Exiting Address()");
        City = city;
		_logger.LogDebug("Address(street={Street},city={City},state={State},country={Country},zipcode=<redacted>): City → {City1}", street, city, state, country, City);
        State = state;
		_logger.LogError("Address(street={Street},city={City},state={State},country={Country},zipcode=<redacted>): State → {State1}", street, city, state, country, State);
		_logger.LogDebug("Address(street={Street},city={City},state={State},country={Country},zipcode=<redacted>): State → {State1}", street, city, state, country, State);
        Country = country;
        ZipCode = zipcode;
		_logger.LogDebug("Address(street={Street},city={City},state={State},country={Country},zipcode=<redacted>): Country → {Country1}", street, city, state, country, Country);
    _logger.LogWarning("<<< Exiting Address(street={Street},city={City},state={State},country={Country},zipcode=<redacted>)", street, city, state, country);
    }
}
