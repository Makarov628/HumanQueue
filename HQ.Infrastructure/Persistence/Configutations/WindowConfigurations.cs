

using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.UserAggregate.ValueObjects;
using HQ.Domain.WindowAggregate;
using HQ.Domain.WindowAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HQ.Infrastructure.Persistence.Configurations;

public class WindowConfigurations : IEntityTypeConfiguration<WindowAggregate>
{
    public void Configure(EntityTypeBuilder<WindowAggregate> builder)
    {
        ConfigureWindowsTable(builder);
    }

    public void ConfigureWindowsTable(EntityTypeBuilder<WindowAggregate> builder)
    {
        builder.ToTable("Windows");

        builder.HasKey("Id");
        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => WindowId.Create(value)
            );

        builder.Property(s => s.QueueId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QueueId.Create(value)
            );

        builder.Property(s => s.AttachedUserId)
            .ValueGeneratedNever()
            .HasConversion<Guid?>(
                id => id == null ? null : id.Value,
                guid => guid.HasValue ? UserId.Create(guid.Value) : null
            );



    }
}
