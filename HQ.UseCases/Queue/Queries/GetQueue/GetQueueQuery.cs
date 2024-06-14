

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Queue.Queries.GetQueue;

public record GetQueueQuery(
    Guid Id
) : IRequest<ErrorOr<QueueResponse>>;


public record QueueResponse(
    Guid Id,
    string Name,
    string DefaultCulture,
    List<QueueTerminalResponse> Terminals,
    List<QueueWindowResponse> Windows,
    List<QueueServiceResponse> Services
);

public record QueueTerminalResponse(
    Guid Id,
    string Name,
    string? ExternalPrinterId
);

public record QueueWindowResponse(
    Guid Id,
    int Number
);

public record QueueServiceResponse(
    Guid Id,
    string Name,
    int RequestNumberCounter,
    string? Literal,
    Guid? ParentId,
    List<Guid> LinkedWindowsIds,
    List<QueueServiceResponse> Childs
);

public static class ServiceResponseExtension
{
    public static List<QueueServiceResponse> CreateTree(this List<QueueServiceResponse> collection, Guid? parentId = null)
    {
        List<QueueServiceResponse> level = new();
        foreach (QueueServiceResponse node in collection.Where(service => service.ParentId == parentId))
        {
            level.Add(new QueueServiceResponse(
                node.Id,
                node.Name,
                node.RequestNumberCounter,
                node.Literal,
                node.ParentId,
                node.LinkedWindowsIds,
                collection.CreateTree(node.Id)
            ));
        }

        return level;
    }
}