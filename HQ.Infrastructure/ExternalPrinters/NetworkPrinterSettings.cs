

namespace HQ.Infrastructure.ExternalPrinters;

public class NetworkPrinterSettings
{
    public string Id { get; init; } = null;
    public string Name { get; init; } = null;
    public string Host { get; init; } = null;
    public int Port { get; init; } = 9100;
}