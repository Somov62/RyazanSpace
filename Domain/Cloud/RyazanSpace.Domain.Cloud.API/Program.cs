using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Cloud.Services;
using RyazanSpace.Interfaces.Cloud;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.Services.YandexCloud;
using System.Text;

namespace RyazanSpace.Domain.Cloud.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(option =>
                {
                    option.RouteTemplate = "api/Cloud/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/api/Cloud/v1/swagger.json", "Cloud API");
                    option.RoutePrefix = "api/Cloud";
                });
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    StringBuilder message = new();

                    foreach (var value in actionContext.ModelState.Values)
                    {
                        foreach (var error in value.Errors)
                        {
                            if (string.IsNullOrEmpty(error.ErrorMessage)) continue;
                            message.AppendLine(error.ErrorMessage);
                        }
                    }
                    return new BadRequestObjectResult(message.ToString());
                };
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddHttpClient<WebAuthService>
                (configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["AuthAPI"]}/Auth/"); });

            builder.Services.AddHttpClient<IRepository<CloudResource>, WebRepository<CloudResource>>
                (configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Resources/"); });

            builder.Services.AddHttpClient<ICloud, YandexCloudProvider>();

            builder.Services.AddScoped<CloudService>();

        }
    }
}