using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entidades;

namespace Veterinaria.Infrastructure;

/// <summary>
/// Seeder inicial para garantizar la existencia de tipos de usuario, catálogos base y usuario administrador (DER v2).
/// </summary>
public static class InicializadorDatos
{
    public static async Task InicializarAsync(ContextoVeterinaria context)
    {
        // 1. Asegurar la creación del esquema de base de datos
        await context.Database.EnsureCreatedAsync();

        // 2. Sembrar Tipos de Usuario si la tabla está vacía
        if (!await context.TiposUsuario.AnyAsync())
        {
            context.TiposUsuario.AddRange(
                new TipoUsuario { Nombre = "Administrador", Activo = true },
                new TipoUsuario { Nombre = "Veterinario", Activo = true },
                new TipoUsuario { Nombre = "Secretario", Activo = true }
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

        // 5. Sembrar usuarios de demostración por rol si no existen
        await SembrarUsuarioSiNoExisteAsync(context, "Administrador", "admin", "admin123", "Administrador", "Sistema", "12345678", "ADM-001");
        await SembrarUsuarioSiNoExisteAsync(context, "Veterinario", "vet", "vet123", "Lucía", "Pérez", "23456789", "MN-1024");
        await SembrarUsuarioSiNoExisteAsync(context, "Secretario", "secretario", "sec123", "Martín", "Gómez", "34567890", "REC-001");
    }

    private static async Task SembrarUsuarioSiNoExisteAsync(
        ContextoVeterinaria context,
        string nombreTipo,
        string nombreUsuario,
        string contrasena,
        string nombre,
        string apellido,
        string dni,
        string matricula)
    {
        var tipo = await context.TiposUsuario.FirstOrDefaultAsync(t => t.Nombre == nombreTipo);
        if (tipo is null)
        {
            return;
        }

        var existe = await context.Usuarios.AnyAsync(u => u.NombreUsuario.ToLower() == nombreUsuario);
        if (existe)
        {
            return;
        }

        context.Usuarios.Add(new Usuario
        {
            IdTipoUsuario = tipo.Id,
            NombreUsuario = nombreUsuario,
            HashContrasena = BCrypt.Net.BCrypt.HashPassword(contrasena),
            Nombre = nombre,
            Apellido = apellido,
            DNI = dni,
            Matricula = matricula,
            Activo = true
        });

        await context.SaveChangesAsync();
    }
}
