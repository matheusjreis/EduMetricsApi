using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduMetricsApi.Domain.Entities;

namespace EduMetricsApi.Infra.Data.Configurations;

public abstract class BaseConfiguration
{
    protected void ConfigureBaseEntity<T>(EntityTypeBuilder<T> builder, bool ignoreBaseEntity = false) where T : class
    {
        if (!ignoreBaseEntity && typeof(T).BaseType == typeof(EntityBase))
        {
            throw new Exception($"Você está tentando configurar um {nameof(EntityBase)} com o configuration errado.");
        }

        var propId = typeof(T).GetProperty(nameof(EntityBase.Id));
        if (propId != null && propId.PropertyType == typeof(Guid))
        {
            builder.HasKey(propId.Name);
            builder.Property(propId.Name)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            return;
        }

        throw new Exception($"{typeof(T)} - Propriedade Id não encontrada.");
    }
}

public abstract class BaseConfiguration<T> : BaseConfiguration, IEntityTypeConfiguration<T>
    where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        base.ConfigureBaseEntity(builder, true);

        builder.HasIndex(x => x.Id).IsUnique();
    }
}