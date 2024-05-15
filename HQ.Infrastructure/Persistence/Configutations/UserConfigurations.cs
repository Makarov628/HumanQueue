

using HQ.Domain.UserAggregate;
using HQ.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HQ.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
       ConfigureUsersTable(builder);
    }

    public void ConfigureUsersTable(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("Users");

        builder.HasKey("Id");
        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            );

    }
}
