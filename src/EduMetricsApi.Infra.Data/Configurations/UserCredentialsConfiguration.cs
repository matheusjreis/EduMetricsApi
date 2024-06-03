using EduMetricsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduMetricsApi.Infra.Data.Configurations;

public class UserCredentialsConfiguration : BaseConfiguration<UserCredentials>
{
    public override void Configure(EntityTypeBuilder<UserCredentials> builder)
    {
        builder.HasKey(x => x.Id);
    }    
}