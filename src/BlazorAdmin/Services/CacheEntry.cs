using Microsoft.Extensions.Logging;
using System;

namespace BlazorAdmin.Services;

public class CacheEntry<T>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CacheEntry(T item)
    {
		_logger.LogInformation("ENTERING: CacheEntry() method");
        Value = item;
    }
    public CacheEntry()
    {

    }

    public T Value { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
