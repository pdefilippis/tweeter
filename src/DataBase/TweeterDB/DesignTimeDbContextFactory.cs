using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; // Para leer appsettings.json si es necesario
using System.IO;
using TweeterDB.Context;

namespace TweeterDB
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TweetContext>
    {
        public TweetContext CreateDbContext(string[] args)
        {
            // Opcional: Si quieres leer la cadena de conexión desde un appsettings.json
            // que esté en tu proyecto de librería para tiempo de diseño.
            // IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory()) // Asegúrate que la base sea la de tu proyecto de librería
            //    .AddJsonFile("appsettings.Development.json", optional: true) // O el appsettings que uses para diseño
            //    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TweetContext>();

            // Aquí debes poner tu cadena de conexión.
            // Puedes hardcodearla para tiempo de diseño (no ideal para producción, pero sirve para migraciones)
            // o leerla de alguna forma.
            // Ejemplo hardcodeado (¡RECUERDA CAMBIAR ESTO POR TU CADENA REAL!):
            var connectionString = "Host=localhost;Port=5432;Database=tweeter;Username=usrtweeter;Password=123456";
            // var connectionString = configuration.GetConnectionString("TuConnectionStringName"); // Si lees de appsettings

            optionsBuilder.UseNpgsql(connectionString);

            return new TweetContext(optionsBuilder.Options);
        }
    }
}
