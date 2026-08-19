using Microsoft.Extensions.Logging;
﻿namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

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
		_logger.LogDebug(">>> Entering Address(street={Street},city={City},state={State},country={Country},zipcode={Zipcode})", street, city, state, country, zipcode);
        Street = street;
        City = city;
		_logger.LogDebug("Address(street={Street},city={City},state={State},country={Country},zipcode={Zipcode}): City → {City1}", street, city, state, country, zipcode, City);
        State = state;
        Country = country;
		_logger.LogDebug("Address(street={Street},city={City},state={State},country={Country},zipcode={Zipcode}): State → {State1}", street, city, state, country, zipcode, State);
		_logger.LogDebug("Address(street={Street},city={City},state={State},country={Country},zipcode={Zipcode}): Country → {Country1}", street, city, state, country, zipcode, Country);
		_logger.LogDebug("<<< Exiting Address(street={Street},city={City},state={State},country={Country},zipcode={Zipcode})", street, city, state, country, zipcode);
        ZipCode = zipcode;
    }
}
