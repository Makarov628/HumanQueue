
using Microsoft.EntityFrameworkCore;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;

namespace HQ.Infrastructure.Persistence.Repositories;

public class TerminalRepository : ITerminalRepository
{
    private readonly HQDbContext _dbContext;

    public TerminalRepository(HQDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(TerminalAggregate terminal, CancellationToken cancellationToken)
    {
        _dbContext.Terminals.Add(terminal);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(TerminalAggregate terminal, CancellationToken cancellationToken)
    {
        _dbContext.Terminals.Remove(terminal);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TerminalAggregate?> GetTerminal(TerminalId terminalId, CancellationToken cancellationToken)
    {
        return await _dbContext.Terminals.FirstOrDefaultAsync(t => t.Id == terminalId, cancellationToken);
    }

    public async Task<List<TerminalAggregate>> GetTerminalsByQueue(QueueId queueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Terminals
            .Where(t => t.QueueId == queueId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsExists(TerminalId terminalId, CancellationToken cancellationToken)
    {
        return await _dbContext.Terminals.AnyAsync(t => t.Id == terminalId, cancellationToken);
    }

    public async Task Update(TerminalAggregate terminal, CancellationToken cancellationToken)
    {
        _dbContext.Terminals.Update(terminal);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}