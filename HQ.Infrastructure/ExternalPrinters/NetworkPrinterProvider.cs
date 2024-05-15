
using System.Text;
using ErrorOr;
using HQ.Application.Printer;
using Microsoft.Extensions.Options;

using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

namespace HQ.Infrastructure.ExternalPrinters;

public class NetworkPrinterProvider : IExternalPrinterProvider
{
    private readonly List<NetworkPrinterSettings> _networkPrinterSettings;

    public NetworkPrinterProvider(IOptions<List<NetworkPrinterSettings>> networkPrinterSettings)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _networkPrinterSettings = networkPrinterSettings.Value;
    }

    public async Task<ErrorOr<Printer>> GetPrinter(string printerId, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (!_networkPrinterSettings.Any(p => p.Id == printerId))
            return Error.NotFound(description: "Принтер не найден");

        return _networkPrinterSettings.ConvertAll(p => new Printer(p.Id, p.Name)).First(p => p.Id == printerId);
    }

    public async Task<ErrorOr<List<Printer>>> GetPrinters(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return _networkPrinterSettings.ConvertAll(p => new Printer(p.Id, p.Name));
    }

    public async Task<ErrorOr<Success>> Print(string printerId, RequestForPrint request, CancellationToken cancellationToken)
    {
        var printer = _networkPrinterSettings.FirstOrDefault(p => p.Id == printerId);
        if (printer is null)
        {
            return Error.NotFound(description: "Принтер не найден");
        }

        try
        {

            var networkPrinter = new ImmediateNetworkPrinter(
                new ImmediateNetworkPrinterSettings()
                {
                    ConnectionString = $"{printer.Host}:{printer.Port}",
                    PrinterName = printer.Name,
                    ConnectTimeoutMs = 3000
                }
            );

            EPSON emitter = new();
            bool isOnline = await networkPrinter.GetOnlineStatus(emitter);
            if (!isOnline)
            {
                return Error.Failure(description: $"Принтер '{printer.Name}' не в сети.");
            }

            await networkPrinter.WriteAsync(
                ByteSplicer.Combine(
                    emitter.CodePage(CodePage.HIRAGANA),
                    emitter.CenterAlign(),
                    emitter.PrintLine(""),
                    PrepareForPrint($"{request.ServiceName}"),
                    emitter.PrintLine(""),
                    emitter.PrintLine(""),
                    PrepareForPrint($"Ваш номер очереди"),
                    emitter.PrintLine(""),
                    PrepareForPrint($"{request.Number}"),
                    emitter.PrintLine(""),
                    emitter.PrintLine(""),
                    PrepareForPrint($"Дата и время выдачи талона"),
                    emitter.PrintLine(""),
                    PrepareForPrint($"{request.CreatedDate}"),
                    emitter.PrintLine(""),
                    emitter.PrintLine(""),
                    emitter.PrintLine(""),
                    emitter.PartialCut()
                )
            );
        }
        catch (Exception e)
        {
            return Error.Unexpected(description: $"Не удалось напечатать талон: {e.Message}");
        }

        return Result.Success;

    }

    private byte[] PrepareForPrint(string value)
    {
        Encoding utf8 = Encoding.UTF8;
        Encoding win1251 = Encoding.GetEncoding(1251);
        byte[] utf8Bytes = utf8.GetBytes(value);
        byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
        return win1251Bytes;
    }
}
