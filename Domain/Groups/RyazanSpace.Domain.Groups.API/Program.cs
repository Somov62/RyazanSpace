using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Client.Repositories.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Groups.Services;
using RyazanSpace.Interfaces.Repositories;
using System.Text;

namespace RyazanSpace.Domain.Groups.API
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
                    option.RouteTemplate = "api/Group/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/api/Group/v1/swagger.json", "Groups API");
                    option.RoutePrefix = "api/Group";
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

            builder.Services.AddHttpClient<WebGroupsRepository>
                (configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Groups/"); });

            builder.Services.AddHttpClient<WebSubscribeRepository>
                (configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Subscriber/"); });

            builder.Services.AddHttpClient<WebPostRepository>
                (configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Post/"); });

            builder.Services.AddHttpClient<IRepository<CloudResource>, WebRepository<CloudResource>>
                (configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Resource/"); });

            builder.Services.AddScoped<GroupService>();
            builder.Services.AddScoped<PostService>();
            builder.Services.AddScoped<SubscribeService>();

        }
    }
}