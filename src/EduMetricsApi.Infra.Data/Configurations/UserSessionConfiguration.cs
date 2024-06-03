using EduMetricsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduMetricsApi.Infra.Data.Configurations;

public class UserSessionConfiguration : BaseConfiguration<UserSession>
{
    public override void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(x => x.Id);
    }
}