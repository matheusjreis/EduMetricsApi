using EduMetricsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduMetricsApi.Infra.Data.Configurations;

public class UserRegisterConfiguration : BaseConfiguration<UserRegister>
{
    public override void Configure(EntityTypeBuilder<UserRegister> builder)
    {
        builder.HasKey(x => x.Id);
    }
}