using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entidades;

namespace Veterinaria.Infrastructure;

/// <summary>
/// Seeder inicial para garantizar la existencia de roles maestros, catálogos base y usuario administrador.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task InicializarAsync(VeterinariaDbContext context)
    {
        // 1. Asegurar la creación del esquema de base de datos
        await context.Database.EnsureCreatedAsync();

        // 2. Sembrar Roles Maestros si la tabla está vacía
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Rol { Nombre = "Administrador", Activo = true },
                new Rol { Nombre = "Veterinario", Activo = true },
                new Rol { Nombre = "Secretario", Activo = true }
            );

            await context.SaveChangesAsync();
        }

        // 3. Sembrar Especies iniciales
        if (!await context.Especies.AnyAsync())
        {
            context.Especies.AddRange(
                new Especie { Nombre = "Canino", Activo = true },
                new Especie { Nombre = "Felino", Activo = true },
                new Especie { Nombre = "Ave", Activo = true },
                new Especie { Nombre = "Roedor", Activo = true }
            );

            await context.SaveChangesAsync();
        }

        // 4. Sembrar Métodos de Pago iniciales
        if (!await context.MetodosPago.AnyAsync())
        {
            context.MetodosPago.AddRange(
                new MetodoPago { Nombre = "Efectivo", Activo = true },
                new MetodoPago { Nombre = "Tarjeta de Débito", Activo = true },
                new MetodoPago { Nombre = "Tarjeta de Crédito", Activo = true },
                new MetodoPago { Nombre = "Transferencia Bancaria", Activo = true }
            );

            await context.SaveChangesAsync();
        }

        // 5. Sembrar Usuario Administrador inicial si no existe
        var rolAdmin = await context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Administrador");
        if (rolAdmin != null)
        {
            var adminExiste = await context.Usuarios.AnyAsync(u => u.Username.ToLower() == "admin");
            if (!adminExiste)
            {
                var adminUsuario = new Usuario
                {
                    IdRol = rolAdmin.Id,
                    Username = "admin",
                    PasswordHash = "admin123",
                    Nombre = "Administrador",
                    Apellido = "Sistema",
                    DNI = "12345678",
                    Matricula = "ADM-001",
                    Activo = true
                };

                context.Usuarios.Add(adminUsuario);
                await context.SaveChangesAsync();
            }
        }
    }
}
