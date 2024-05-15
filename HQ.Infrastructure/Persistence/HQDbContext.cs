using HQ.Domain.Common.Models;
using HQ.Domain.QueueAggregate;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.UserAggregate;
using HQ.Domain.WindowAggregate;
using HQ.Infrastructure.Persistence.Configurations;
using HQ.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace HQ.Infrastructure.Persistence;

public class HQDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public HQDbContext(DbContextOptions<HQDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<QueueAggregate> Queues { get; set; } = null!;
    public DbSet<ServiceAggregate> Services { get; set; } = null!;
    public DbSet<TerminalAggregate> Terminals { get; set; } = null!;
    public DbSet<UserAggregate> Users { get; set; } = null!;
    public DbSet<WindowAggregate> Windows { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<List<IDomainEvent>>();

        modelBuilder.ApplyConfiguration(new QueueConfigurations());
        modelBuilder.ApplyConfiguration(new ServiceConfigurations());
        modelBuilder.ApplyConfiguration(new TerminalConfigurations());
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new WindowConfigurations());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}