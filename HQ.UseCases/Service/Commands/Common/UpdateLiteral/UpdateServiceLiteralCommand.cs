


using ErrorOr;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.UpdateLiteral;

public record UpdateServiceLiteralCommand(
    Guid Id,
    string Litetal
): IRequest<ErrorOr<Success>>;