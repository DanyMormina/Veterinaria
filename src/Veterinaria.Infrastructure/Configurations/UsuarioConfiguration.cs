using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinaria.Domain.Entidades;

namespace Veterinaria.Infrastructure.Configurations;

/// <summary>
/// Configuración Fluent API de Entity Framework Core para la entidad Usuario.
/// </summary>
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.PuedeRestaurarBD)
            .HasDefaultValue(false)
            .IsRequired();
    }
}
