using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.API.Data;
using RyazanSpace.DAL.Repositories.Account;
using RyazanSpace.DAL.Repositories.Base;
using RyazanSpace.Interfaces.Repositories;
using System.Text.Json.Serialization;

namespace RyazanSpace.DAL.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<RyazanSpaceDbInitializer>().Initialize();
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(option =>
                {
                    option.RouteTemplate = "api/Database/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/api/Database/v1/swagger.json", "Database API");
                    option.RoutePrefix = "api/Database";
                });
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddJsonOptions(p => p.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RyazanSpaceDbContext>(
                opt => opt
                    .UseSqlServer(builder.Configuration.GetConnectionString("Data"),
                    o => o.MigrationsAssembly("RyazanSpace.DAL.SqlServer")));

            builder.Services.AddTransient<RyazanSpaceDbInitializer>();

            AddRepositories(builder);

        }

        private static void AddRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            builder.Services.AddScoped(typeof(INamedRepository<>), typeof(DbNamedRepository<>));
            builder.Services.AddScoped(typeof(DbUserRepository));
        }
    }
}