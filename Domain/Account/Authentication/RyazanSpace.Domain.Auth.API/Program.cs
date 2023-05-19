using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.WebApiClients.Repositories.Account;
using RyazanSpace.DAL.WebApiClients.Repositories.Base;
using RyazanSpace.Domain.Auth.Services;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.MailService;
using System.Net;

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddHttpClient<WebUserRepository>
                ( configureClient :
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Users/"); });

            builder.Services.AddHttpClient<IRepository<EmailVerificationSession>, WebRepository<EmailVerificationSession>> 
                ( configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/EmailVerificationSessions/"); });

            builder.Services.AddHttpClient<IRepository<ResetPasswordSession>, WebRepository<ResetPasswordSession>>
               (configureClient:
               client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/ResetPasswordSessions/"); });


            builder.Services.AddScoped<RegistrationService>();
            builder.Services.AddScoped<EmailVerificationService>();
            builder.Services.AddScoped<ResetPasswordService>();
            builder.Services.AddScoped(c => new EmailSender(
                new NetworkCredential(
                    builder.Configuration["EmailSenderUserName"], 
                    builder.Configuration["EmailSenderPassword"])));

        }
    }
}