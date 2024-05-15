

using ErrorOr;

namespace HQ.Application.Printer;

public record Printer(
    string Id,
    string Name
);

public record RequestForPrint(
    Guid Id,
    string Number,
    string ServiceName,
    DateTime CreatedDate
);

public interface IExternalPrinterProvider
{
    Task<ErrorOr<Success>> Print(string printerId, RequestForPrint request, CancellationToken cancellationToken);
    Task<ErrorOr<List<Printer>>> GetPrinters(CancellationToken cancellationToken);
    Task<ErrorOr<Printer>> GetPrinter(string printerId, CancellationToken cancellationToken);
}