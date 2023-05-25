using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Client.Repositories.Credentials;
using RyazanSpace.Domain.Auth.Services;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.Services.MailService;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using RyazanSpace.Interfaces.Email;

namespace RyazanSpace.Domain.Auth.API
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
                    option.RouteTemplate = "api/Authentication/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/api/Authentication/v1/swagger.json", "Auth API");
                    option.RoutePrefix = "api/Authentication";
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


            builder.Services.AddHttpClient<WebUserRepository>
                ( configureClient :
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Users/"); });

            builder.Services.AddHttpClient<WebUserTokenRepository>
               (configureClient:
               client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/UserToken/"); });

            builder.Services.AddHttpClient<IRepository<EmailVerificationSession>, WebRepository<EmailVerificationSession>> 
                ( configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/EmailVerificationSessions/"); });

            builder.Services.AddHttpClient<IRepository<ResetPasswordSession>, WebRepository<ResetPasswordSession>>
               (configureClient:
               client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/ResetPasswordSessions/"); });

            builder.Services.AddScoped<RegistrationService>();
            builder.Services.AddScoped<EmailVerificationService>();
            builder.Services.AddScoped<ResetPasswordService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>(c => new EmailSender(
                new NetworkCredential(
                    builder.Configuration["EmailSenderUserName"], 
                    builder.Configuration["EmailSenderPassword"])));

        }
    }
}