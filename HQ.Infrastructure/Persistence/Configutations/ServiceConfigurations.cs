

using System.Globalization;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.Enums;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HQ.Infrastructure.Persistence.Configurations;

public class ServiceConfigurations : IEntityTypeConfiguration<ServiceAggregate>
{
    public void Configure(EntityTypeBuilder<ServiceAggregate> builder)
    {
        ConfigureServicesTable(builder);
        ConfigureServiceRequestsTable(builder);
        ConfigureServiceWindowLinksTable(builder);
    }

    public void ConfigureServicesTable(EntityTypeBuilder<ServiceAggregate> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(nameof(ServiceAggregate.Id));
        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ServiceId.Create(value)
            );

        builder.Property(s => s.QueueId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QueueId.Create(value)
            );

        builder.Property(s => s.Name)
            .ValueGeneratedNever()
            .HasConversion(
                localizedName => localizedName.ToJsonString(),
                value => LocalizedString.CreateFromJson(value)
            );

        builder.Property(s => s.Literal)
            .ValueGeneratedNever()
            .HasConversion<string?>(
                literal => literal == null ? null : literal.Value,
                stringValue => !string.IsNullOrEmpty(stringValue) ? ServiceLiteral.Create(stringValue!).Value : null
            );

        builder.Property(s => s.ParentId)
            .ValueGeneratedNever()
            .HasConversion<Guid?>(
                parentId => parentId == null ? null : parentId.Value,
                guid => guid.HasValue ? ServiceId.Create(guid.Value) : null
            );
    }

    public void ConfigureServiceRequestsTable(EntityTypeBuilder<ServiceAggregate> builder)
    {
        builder.OwnsMany(s => s.Requests, rb =>
        {
            rb.ToTable("ServiceRequests");
            
            rb.WithOwner().HasForeignKey("ParentServiceId").HasPrincipalKey(s => s.Id);

            rb.HasKey("Id", "ParentServiceId");

            rb.Property(r => r.Id)
                .HasColumnName("RequestId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => RequestId.Create(value)
                );

            rb.Property(r => r.Number)
                .ValueGeneratedNever()
                .HasConversion(
                    number => number.ConvertToPersistString(),
                    stringValue => RequestNumber.BuildFromString(stringValue)
                );

            rb.Property(r => r.Status)
                .ValueGeneratedNever()
                .HasConversion(
                    status => status.Value,
                    intValue => RequestStatus.FromValue(intValue)
                );

            rb.Property(r => r.Culture)
                .ValueGeneratedNever()
                .HasConversion(
                    culture => culture.Name,
                    stringValue => Culture.Create(stringValue).Value
                );

            rb.Property(r => r.CreatedFromTerminalId)
                .ValueGeneratedNever()
                .HasConversion(
                    terminalId => terminalId.Value,
                    guid => TerminalId.Create(guid)
                );

            rb.Property(r => r.CalledByWindowId)
                .ValueGeneratedNever()
                .HasConversion<Guid?>(
                    windowId => windowId == null ? null : windowId.Value,
                    guid => guid.HasValue ? WindowId.Create(guid.Value) : null
                );

            rb.Property(r => r.CreatedForServiceId)
                .ValueGeneratedNever()
                .HasConversion(
                    terminalId => terminalId.Value,
                    guid => ServiceId.Create(guid)
                );
        });

        builder.Navigation(s => s.Requests).Metadata.SetField("_requests");
        builder.Navigation(s => s.Requests).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    public void ConfigureServiceWindowLinksTable(EntityTypeBuilder<ServiceAggregate> builder)
    {
        builder.OwnsMany(s => s.WindowLinks, wlb => 
        {
            wlb.ToTable("ServiceWindowLinks");
            
            wlb.WithOwner().HasForeignKey("ServiceId");

            wlb.HasKey("Id");
            wlb.Property(wl => wl.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ServiceId.Create(value)
                );

            wlb.Property(wl => wl.WindowId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => WindowId.Create(value)
                );

        });

        builder.Navigation(s => s.WindowLinks).Metadata.SetField("_windowLinks");
        builder.Navigation(s => s.WindowLinks).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
