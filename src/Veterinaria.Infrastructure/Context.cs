using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Entidades;

namespace Veterinaria.Infrastructure;

/// <summary>
/// Contexto principal de Entity Framework Core para la base de datos de Veterinaria.
/// Incluye mapeo Fluent segmentado por módulos, soft delete global y filtros automáticos.
/// </summary>
public class VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> options) : DbContext(options)
{
    // =========================================================================
    // 1. Catálogos Maestros y Satélites
    // =========================================================================
    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Raza> Razas => Set<Raza>();
    public DbSet<Vacuna> Vacunas => Set<Vacuna>();
    public DbSet<MetodoPago> MetodosPago => Set<MetodoPago>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();

    // =========================================================================
    // 2. Seguridad y Usuarios
    // =========================================================================
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    // =========================================================================
    // 3. Clientes y Pacientes
    // =========================================================================
    public DbSet<Propietario> Propietarios => Set<Propietario>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();

    // =========================================================================
    // 4. Clínica y Consultas Médicas
    // =========================================================================
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<DetalleConsulta> DetalleConsultas => Set<DetalleConsulta>();
    public DbSet<AplicacionVacuna> AplicacionesVacuna => Set<AplicacionVacuna>();

    // =========================================================================
    // 5. Pagos y Cobranzas
    // =========================================================================
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeterinariaDbContext).Assembly);

        // ---------------------------------------------------------------------
        // Filtro Global de Soft Delete: Toda entidad Auditable filtra por Activo == true
        // ---------------------------------------------------------------------
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Auditable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(Auditable.Activo));
                var condition = Expression.Equal(property, Expression.Constant(true));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        // ---------------------------------------------------------------------
        // Construcción y Mapeo Fluent de Entidades por Módulos
        // ---------------------------------------------------------------------
        BuildCatalogos(modelBuilder);
        BuildSeguridad(modelBuilder);
        BuildClientesPacientes(modelBuilder);
        BuildClinicaConsultas(modelBuilder);
        BuildPagos(modelBuilder);
    }

    /// <summary>
    /// Mapeo de entidades de Catálogo y Datos Maestros.
    /// </summary>
    private static void BuildCatalogos(ModelBuilder builder)
    {
        // TipoUsuario
        builder.Entity<TipoUsuario>(b =>
        {
            b.ToTable("TipoUsuario");
            b.Property(t => t.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Especie
        builder.Entity<Especie>(b =>
        {
            b.ToTable("Especie");
            b.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Raza
        builder.Entity<Raza>(b =>
        {
            b.ToTable("Raza");
            b.Property(r => r.Nombre)
                .HasMaxLength(80)
                .IsRequired();

            b.HasOne(r => r.Especie)
                .WithMany(e => e.Razas)
                .HasForeignKey(r => r.IdEspecie)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Vacuna
        builder.Entity<Vacuna>(b =>
        {
            b.ToTable("Vacuna");
            b.Property(v => v.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(v => v.PeriodoMesesRecomendado)
                .HasDefaultValue(12);
        });

        // MetodoPago
        builder.Entity<MetodoPago>(b =>
        {
            b.ToTable("MetodoPago");
            b.Property(m => m.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Tratamiento (Catálogo Maestro)
        builder.Entity<Tratamiento>(b =>
        {
            b.ToTable("Tratamiento");
            b.Property(t => t.TipoTratamiento)
                .HasMaxLength(50)
                .IsRequired();

            b.Property(t => t.Descripcion)
                .IsRequired();

            b.Property(t => t.Dosis)
                .HasMaxLength(100);

            b.Property(t => t.Precio)
                .HasColumnType("decimal(18,2)");
        });
    }

    /// <summary>
    /// Mapeo de entidades de Seguridad y Usuarios.
    /// </summary>
    private static void BuildSeguridad(ModelBuilder builder)
    {
        // Usuario
        builder.Entity<Usuario>(b =>
        {
            b.ToTable("Usuario");
            b.HasIndex(u => u.Username)
                .IsUnique();

            b.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            b.Property(u => u.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            b.Property(u => u.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(u => u.Apellido)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(u => u.DNI)
                .HasMaxLength(20)
                .IsRequired();

            b.Property(u => u.Matricula)
                .HasMaxLength(50);

            b.HasOne(u => u.TipoUsuario)
                .WithMany(t => t.Usuarios)
                .HasForeignKey(u => u.IdTipoUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Mapeo de entidades de Clientes (Propietarios) y Pacientes (Mascotas).
    /// </summary>
    private static void BuildClientesPacientes(ModelBuilder builder)
    {
        // Propietario
        builder.Entity<Propietario>(b =>
        {
            b.ToTable("Propietario");
            b.HasIndex(p => p.DNI)
                .IsUnique();

            b.Property(p => p.DNI)
                .HasMaxLength(20)
                .IsRequired();

            b.Property(p => p.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(p => p.Apellido)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(p => p.Telefono)
                .HasMaxLength(30);

            b.Property(p => p.Email)
                .HasMaxLength(100);

            b.Property(p => p.Direccion)
                .HasMaxLength(200);

            b.HasMany(p => p.Mascotas)
                .WithOne(m => m.Propietario)
                .HasForeignKey(m => m.IdPropietario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Mascota
        builder.Entity<Mascota>(b =>
        {
            b.ToTable("Mascota");
            b.Property(m => m.Nombre)
                .HasMaxLength(80)
                .IsRequired();

            b.Property(m => m.Sexo)
                .HasMaxLength(10)
                .IsRequired();

            b.Property(m => m.Color)
                .HasMaxLength(50);

            b.HasOne(m => m.Propietario)
                .WithMany(p => p.Mascotas)
                .HasForeignKey(m => m.IdPropietario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(m => m.Raza)
                .WithMany(r => r.Mascotas)
                .HasForeignKey(m => m.IdRaza)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Mapeo de entidades de Atención Clínica, Consultas, Detalles de Tratamiento y Vacunación.
    /// </summary>
    private static void BuildClinicaConsultas(ModelBuilder builder)
    {
        // Consulta
        builder.Entity<Consulta>(b =>
        {
            b.ToTable("Consulta");
            b.HasIndex(c => c.FechaHora);

            b.Property(c => c.Motivo)
                .HasMaxLength(250);

            b.Property(c => c.PesoKg)
                .HasColumnType("decimal(6,2)");

            b.Property(c => c.Temperatura)
                .HasColumnType("decimal(4,2)");

            b.Property(c => c.Diagnostico)
                .IsRequired();

            b.HasOne(c => c.Mascota)
                .WithMany(m => m.Consultas)
                .HasForeignKey(c => c.IdMascota)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.Usuario)
                .WithMany(u => u.Consultas)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(c => c.DetallesConsulta)
                .WithOne(d => d.Consulta)
                .HasForeignKey(d => d.IdConsulta)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(c => c.AplicacionesVacuna)
                .WithOne(a => a.Consulta)
                .HasForeignKey(a => a.IdConsulta)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(c => c.Pagos)
                .WithOne(p => p.Consulta)
                .HasForeignKey(p => p.IdConsulta)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // DetalleConsulta
        builder.Entity<DetalleConsulta>(b =>
        {
            b.ToTable("DetalleConsulta");
            b.Property(d => d.Cantidad)
                .HasDefaultValue(1);

            b.Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(18,2)");

            b.Property(d => d.Subtotal)
                .HasColumnType("decimal(18,2)");

            b.HasOne(d => d.Consulta)
                .WithMany(c => c.DetallesConsulta)
                .HasForeignKey(d => d.IdConsulta)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(d => d.Tratamiento)
                .WithMany(t => t.DetallesConsulta)
                .HasForeignKey(d => d.IdTratamiento)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AplicacionVacuna
        builder.Entity<AplicacionVacuna>(b =>
        {
            b.ToTable("AplicacionVacuna");
            b.Property(a => a.Observaciones)
                .HasMaxLength(250);

            b.Property(a => a.PrecioAplicado)
                .HasColumnType("decimal(18,2)");

            b.HasOne(a => a.Consulta)
                .WithMany(c => c.AplicacionesVacuna)
                .HasForeignKey(a => a.IdConsulta)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(a => a.Vacuna)
                .WithMany(v => v.AplicacionesVacuna)
                .HasForeignKey(a => a.IdVacuna)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Mapeo de entidades de Pagos y Cobranzas.
    /// </summary>
    private static void BuildPagos(ModelBuilder builder)
    {
        // Pago
        builder.Entity<Pago>(b =>
        {
            b.ToTable("Pago");
            b.HasIndex(p => p.Fecha);

            b.Property(p => p.Importe)
                .HasColumnType("decimal(18,2)");

            b.Property(p => p.Estado)
                .HasMaxLength(30)
                .HasDefaultValue("Completado");

            b.HasOne(p => p.Consulta)
                .WithMany(c => c.Pagos)
                .HasForeignKey(p => p.IdConsulta)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(p => p.MetodoPago)
                .WithMany(m => m.Pagos)
                .HasForeignKey(p => p.IdMetodoPago)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Intercepta el guardado de cambios para aplicar soft delete defensivo.
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Auditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Activo = true;
                    break;

                case EntityState.Deleted:
                    // Interceptar eliminación física y transformar en Soft Delete
                    entry.State = EntityState.Modified;
                    entry.Entity.Activo = false;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}