

using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HQ.Infrastructure.Persistence.Configurations;

public class TerminalConfigurations : IEntityTypeConfiguration<TerminalAggregate>
{
    public void Configure(EntityTypeBuilder<TerminalAggregate> builder)
    {
        ConfigureTerminalsTable(builder);
    }

    public void ConfigureTerminalsTable(EntityTypeBuilder<TerminalAggregate> builder)
    {
        builder.ToTable("Terminals");

        builder.HasKey("Id");
        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TerminalId.Create(value)
            );

        builder.Property(s => s.QueueId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QueueId.Create(value)
            );

    }
}
