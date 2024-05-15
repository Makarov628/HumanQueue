

using MediatR;
using HQ.Application.Printer;
using ErrorOr;

namespace HQ.UseCases.Terminal.Queries.GetExternalPrinters;

public record GetExternalPrintersQuery(): IRequest<ErrorOr<List<Printer>>>;