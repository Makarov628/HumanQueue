

using ErrorOr;
using HQ.Domain.Common.ValueObjects;
using MediatR;

namespace HQ.UseCases.Terminal.Queries.GetTerminal;

public record GetTerminalQuery(
    Guid Id
): IRequest<ErrorOr<TerminalResponse>>;

public record TerminalResponse(
    Guid Id,
    string Name,
    List<AvailableCultureResponse> AvailableCultures,
    List<TerminalServiceResponse> Services
);

public record AvailableCultureResponse(
    string Culture,
    string LanguageName
);

public record TerminalServiceResponse(
    Guid Id,
    List<CultureString> Name,
    int RequestNumberCounter,
    string? Literal,
    Guid? ParentId,
    List<TerminalServiceResponse> Childs
); 

public static class ServiceResponseExtension
{
    public static List<TerminalServiceResponse> CreateTree(this List<TerminalServiceResponse> collection, Guid? parentId = null)
    {
        List<TerminalServiceResponse> level = new();
        foreach (TerminalServiceResponse node in collection.Where(service => service.ParentId == parentId))
        {
            level.Add(new TerminalServiceResponse(
                node.Id,
                node.Name,
                node.RequestNumberCounter,
                node.Literal,
                node.ParentId,
                collection.CreateTree(node.Id)
            ));
        }

        return level;
    }
}