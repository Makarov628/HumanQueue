

using System.Globalization;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HQ.Infrastructure.Persistence.Configurations;

public class QueueConfigurations : IEntityTypeConfiguration<QueueAggregate>
{
    public void Configure(EntityTypeBuilder<QueueAggregate> builder)
    {
       ConfigureQueuesTable(builder);
    }

    public void ConfigureQueuesTable(EntityTypeBuilder<QueueAggregate> builder)
    {
        builder.ToTable("Queues");

        builder.HasKey(nameof(QueueAggregate.Id));
        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QueueId.Create(value)
            );


        builder.Property(q => q.DefaultCulture)
            .ValueGeneratedNever()
            .HasConversion(
                culture => culture.Name,
                value => Culture.Create(value).Value
            );
    }

    // public void ConfigureQueueServiceIdsTable(EntityTypeBuilder<QueueAggregate> builder)
    // {
    //     builder.OwnsMany(q => q.ServiceIds, sib => 
    //     {
    //         sib.ToTable("QueueServiceIds");
    //         sib.WithOwner().HasForeignKey("QueueId");
            
    //         sib.HasKey("Id");
    //         sib.Property(s => s.Value)
    //             .HasColumnName("ServiceId")
    //             .ValueGeneratedNever();

    //     });

    //     builder.Metadata.FindNavigation(nameof(QueueAggregate.ServiceIds))!
    //         .SetPropertyAccessMode(PropertyAccessMode.Field);
    // }

    // public void ConfigureQueueServiceIdsTable(EntityTypeBuilder<QueueAggregate> builder)
    // {
    //     builder.OwnsMany(q => q.ServiceIds, sib => 
    //     {
    //         sib.ToTable("QueueServiceIds");
    //         sib.WithOwner().HasForeignKey("QueueId");
            
    //         sib.HasKey("Id");
    //         sib.Property(s => s.Value)
    //             .HasColumnName("ServiceId")
    //             .ValueGeneratedNever();

    //     });

    //     builder.Metadata.FindNavigation(nameof(QueueAggregate.ServiceIds))!
    //         .SetPropertyAccessMode(PropertyAccessMode.Field);
    // }
}
