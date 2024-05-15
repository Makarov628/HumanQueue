using MediatR;
using ErrorOr;
using HQ.Domain.Common.ValueObjects;

namespace HQ.UseCases.Terminal.Commands.CreateRequest;

public record TerminalCreateRequestCommand(
    Guid ToServiceId,
    Guid FromTerminalId,
    string Culture
): IRequest<ErrorOr<TerminalRequestResponse>>;

public record TerminalRequestResponse(
    Guid Id,
    string Number,
    string ServiceName,
    string Culture,
    DateTime CreatedDate
);