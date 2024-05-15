

using MediatR;
using HQ.Application.Printer;
using ErrorOr;

namespace HQ.UseCases.Terminal.Queries.GetExternalPrinters;

internal class GetExternalPrintersQueryHandler : IRequestHandler<GetExternalPrintersQuery, ErrorOr<List<Printer>>>
{
      private readonly IExternalPrinterProvider _externalPrinterProvider;

    public GetExternalPrintersQueryHandler(IExternalPrinterProvider externalPrinterProvider)
    {
        _externalPrinterProvider = externalPrinterProvider;
    }

    public async Task<ErrorOr<List<Printer>>> Handle(GetExternalPrintersQuery request, CancellationToken cancellationToken)
    {
        return await _externalPrinterProvider.GetPrinters(cancellationToken);
    }
}
