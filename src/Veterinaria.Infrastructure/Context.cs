using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Entidades;

namespace Veterinaria.Infrastructure;

/// <summary>
/// Contexto principal de Entity Framework Core para la base de datos de Veterinaria.
/// Incluye mapeo Fluent segmentado por módulos, soft delete global y auditoría automática.
/// </summary>
public class VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> options) : DbContext(options)
{
    // =========================================================================
    // 1. Catálogos y Satélites
    // =========================================================================
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Raza> Razas => Set<Raza>();
    public DbSet<Vacuna> Vacunas => Set<Vacuna>();
    public DbSet<MetodoPago> MetodosPago => Set<MetodoPago>();

    // =========================================================================
    // 2. Seguridad y Auditoría
    // =========================================================================
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();

    // =========================================================================
    // 3. Clientes y Pacientes
    // =========================================================================
    public DbSet<Propietario> Propietarios => Set<Propietario>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();

    // =========================================================================
    // 4. Clínica y Agenda
    // =========================================================================
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();

    // =========================================================================
    // 5. Facturación
    // =========================================================================
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<DetalleFactura> DetalleFacturas => Set<DetalleFactura>();

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
        BuildSeguridadAuditoria(modelBuilder);
        BuildClientesPacientes(modelBuilder);
        BuildClinicaAgenda(modelBuilder);
        BuildFacturacion(modelBuilder);
    }

    /// <summary>
    /// Mapeo de entidades de Catálogo y Datos Maestros.
    /// </summary>
    private static void BuildCatalogos(ModelBuilder builder)
    {
        // Rol
        builder.Entity<Rol>(b =>
        {
            b.Property(r => r.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Especie
        builder.Entity<Especie>(b =>
        {
            b.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Raza
        builder.Entity<Raza>(b =>
        {
            b.Property(r => r.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            b.HasOne(r => r.Especie)
                .WithMany()
                .HasForeignKey(r => r.IdEspecie)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Vacuna
        builder.Entity<Vacuna>(b =>
        {
            b.Property(v => v.Nombre)
                .HasMaxLength(100)
                .IsRequired();
        });

        // MetodoPago
        builder.Entity<MetodoPago>(b =>
        {
            b.Property(m => m.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        });
    }

    /// <summary>
    /// Mapeo de entidades de Seguridad, Usuarios, Sesiones y Auditoría.
    /// </summary>
    private static void BuildSeguridadAuditoria(ModelBuilder builder)
    {
        // Usuario
        builder.Entity<Usuario>(b =>
        {
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

            b.HasOne(u => u.Rol)
                .WithMany()
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Sesion
        builder.Entity<Sesion>(b =>
        {
            b.HasIndex(s => s.FechaInicio);

            b.HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Auditoria
        builder.Entity<Auditoria>(b =>
        {
            b.HasIndex(a => a.FechaHora);

            b.Property(a => a.Accion)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(a => a.TablaAfectada)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(a => a.Detalle)
                .HasMaxLength(2000);

            b.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.IdUsuario)
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
                .HasMaxLength(50);

            b.Property(p => p.Email)
                .HasMaxLength(150);

            b.Property(p => p.Direccion)
                .HasMaxLength(250);

            b.HasMany(p => p.Mascotas)
                .WithOne(m => m.Propietario)
                .HasForeignKey(m => m.IdPropietario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Mascota
        builder.Entity<Mascota>(b =>
        {
            b.Property(m => m.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(m => m.Sexo)
                .HasMaxLength(20)
                .IsRequired();

            b.Property(m => m.Color)
                .HasMaxLength(50);

            b.Property(m => m.Peso)
                .HasColumnType("decimal(18,2)");

            b.HasOne(m => m.Propietario)
                .WithMany(p => p.Mascotas)
                .HasForeignKey(m => m.IdPropietario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(m => m.Raza)
                .WithMany()
                .HasForeignKey(m => m.IdRaza)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Mapeo de entidades de Atención Clínica, Turnos, Consultas y Tratamientos.
    /// </summary>
    private static void BuildClinicaAgenda(ModelBuilder builder)
    {
        // Turno
        builder.Entity<Turno>(b =>
        {
            b.HasIndex(t => t.FechaHora);

            b.Property(t => t.Motivo)
                .HasMaxLength(250);

            b.Property(t => t.Estado)
                .HasMaxLength(50)
                .IsRequired();

            b.HasOne(t => t.Mascota)
                .WithMany()
                .HasForeignKey(t => t.IdMascota)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(t => t.Veterinario)
                .WithMany()
                .HasForeignKey(t => t.IdVeterinario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(t => t.Consulta)
                .WithMany()
                .HasForeignKey(t => t.IdConsulta)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Consulta
        builder.Entity<Consulta>(b =>
        {
            b.HasIndex(c => c.FechaHora);

            b.Property(c => c.PesoKg)
                .HasColumnType("decimal(18,2)");

            b.Property(c => c.Temperatura)
                .HasColumnType("decimal(18,2)");

            b.Property(c => c.Diagnostico)
                .HasMaxLength(2000)
                .IsRequired();

            b.Property(c => c.Observaciones)
                .HasMaxLength(2000);

            b.HasOne(c => c.Mascota)
                .WithMany()
                .HasForeignKey(c => c.IdMascota)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.Veterinario)
                .WithMany()
                .HasForeignKey(c => c.IdVeterinario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(c => c.Tratamientos)
                .WithOne(t => t.Consulta)
                .HasForeignKey(t => t.IdConsulta)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Tratamiento
        builder.Entity<Tratamiento>(b =>
        {
            b.Property(t => t.TipoTratamiento)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(t => t.Descripcion)
                .HasMaxLength(1000)
                .IsRequired();

            b.Property(t => t.Dosis)
                .HasMaxLength(200);

            b.Property(t => t.Indicaciones)
                .HasMaxLength(2000);

            b.HasOne(t => t.Consulta)
                .WithMany(c => c.Tratamientos)
                .HasForeignKey(t => t.IdConsulta)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(t => t.Vacuna)
                .WithMany()
                .HasForeignKey(t => t.IdVacuna)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Mapeo de entidades de Facturación y Cobranzas.
    /// </summary>
    private static void BuildFacturacion(ModelBuilder builder)
    {
        // Factura
        builder.Entity<Factura>(b =>
        {
            b.HasIndex(f => f.FechaEmision);

            b.Property(f => f.Total)
                .HasColumnType("decimal(18,2)");

            b.HasOne(f => f.Propietario)
                .WithMany()
                .HasForeignKey(f => f.IdPropietario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(f => f.MetodoPago)
                .WithMany()
                .HasForeignKey(f => f.IdMetodoPago)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(f => f.Detalles)
                .WithOne(d => d.Factura)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DetalleFactura
        builder.Entity<DetalleFactura>(b =>
        {
            b.Property(d => d.Concepto)
                .HasMaxLength(250)
                .IsRequired();

            b.Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(18,2)");

            b.Property(d => d.Subtotal)
                .HasColumnType("decimal(18,2)");

            b.HasOne(d => d.Factura)
                .WithMany(f => f.Detalles)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Intercepta el guardado de cambios para aplicar auditoría automática y soft delete defensivo.
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