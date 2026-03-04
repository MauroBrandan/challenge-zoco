using Microsoft.EntityFrameworkCore;
using backend.Models.Entities;

namespace backend.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Users.Any()) return;

        var admin = new User
        {
            Nombre = "Admin",
            Apellido = "Sistema",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Rol = Role.Admin,
            CreatedAt = DateTime.UtcNow
        };

        var user1 = new User
        {
            Nombre = "Mauro",
            Apellido = "Brandan",
            Email = "mauro@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
            Rol = Role.User,
            CreatedAt = DateTime.UtcNow,
            Estudios = new List<Estudio>
            {
                new() {
                    Institucion = "Universidad Tecnologica Nacional",
                    Titulo = "Ingeniería en Sistemas",
                    NivelEstudio = NivelEstudio.Universitario,
                    FechaInicio = new DateTime(2021, 1, 1),
                    FechaFin = new DateTime(2025, 12, 15)
                }
            },
            Direcciones = new List<Direccion>
            {
                new() {
                    Calle = "Juana Manso 910",
                    Ciudad = "Concepcion",
                    Estado = "Tucuman",
                    Pais = "Argentina",
                    CodigoPostal = "4146"
                }
            }
        };

        var user2 = new User
        {
            Nombre = "Juan Pablo",
            Apellido = "García",
            Email = "juanpablo@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
            Rol = Role.User,
            CreatedAt = DateTime.UtcNow,
            Estudios = new List<Estudio>
            {
                new() {
                    Institucion = "Instituto Tecnológico",
                    Titulo = "Técnico en Programación",
                    NivelEstudio = NivelEstudio.Terciario,
                    FechaInicio = new DateTime(2020, 3, 1),
                    FechaFin = new DateTime(2022, 12, 15)
                },
                new() {
                    Institucion = "Universidad de Córdoba",
                    Titulo = "Licenciatura en Informática",
                    NivelEstudio = NivelEstudio.Universitario,
                    FechaInicio = new DateTime(2023, 3, 1)
                }
            },
            Direcciones = new List<Direccion>
            {
                new() {
                    Calle = "Calle Falsa 123",
                    Ciudad = "Córdoba",
                    Estado = "Córdoba",
                    Pais = "Argentina",
                    CodigoPostal = "X5000"
                }
            }
        };

        context.Users.AddRange(admin, user1, user2);
        context.SaveChanges();
    }
}