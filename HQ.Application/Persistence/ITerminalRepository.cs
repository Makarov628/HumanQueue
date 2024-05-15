

using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;

namespace HQ.Application.Persistence;

public interface ITerminalRepository
{

    Task<TerminalAggregate?> GetTerminal(TerminalId terminalId, CancellationToken cancellationToken);
    Task<List<TerminalAggregate>> GetTerminalsByQueue(QueueId queueId, CancellationToken cancellationToken);

    Task<bool> IsExists(TerminalId terminalId, CancellationToken cancellationToken);

    Task Add(TerminalAggregate terminal, CancellationToken cancellationToken);
    Task Update(TerminalAggregate terminal, CancellationToken cancellationToken);
    Task Delete(TerminalAggregate terminal, CancellationToken cancellationToken);
}