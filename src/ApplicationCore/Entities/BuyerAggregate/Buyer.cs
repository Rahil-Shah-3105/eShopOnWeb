using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.BuyerAggregate;

public class Buyer : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string IdentityGuid { get; private set; }

    private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    #pragma warning disable CS8618 // Required by Entity Framework
    private Buyer() { }

    public Buyer(string identity) : this()
    {
		_logger.LogDebug(">>> Entering Buyer(identity={Identity})", identity);
        Guard.Against.NullOrEmpty(identity, nameof(identity));
        IdentityGuid = identity;
		_logger.LogDebug("<<< Exiting Buyer(identity={Identity})", identity);
		_logger.LogDebug("Buyer(identity={Identity}): IdentityGuid → {IdentityGuid}", identity, IdentityGuid);
    }
}
