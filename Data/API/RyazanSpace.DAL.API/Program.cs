using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.API.Data;
using RyazanSpace.DAL.Repositories.Base;
using RyazanSpace.Interfaces.Repositories;

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
            app.UsePathBase(@"/api/Database");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(option =>
                {
                    option.RouteTemplate = "Database/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/Database/v1/swagger.json", "Database API");
                    option.RoutePrefix = "Database";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
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
        }
    }
}