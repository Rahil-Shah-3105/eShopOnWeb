using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.Web.Pages.Basket;

public class BasketViewModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int Id { get; set; }
    public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
    public string? BuyerId { get; set; }

    public decimal Total()
    {
		_logger.LogInformation("ENTERING: Total() method");
        return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }
}
