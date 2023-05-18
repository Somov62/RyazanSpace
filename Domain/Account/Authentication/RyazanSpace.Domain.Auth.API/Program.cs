using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.WebApiClients.Repositories.Account;
using RyazanSpace.DAL.WebApiClients.Repositories.Base;
using RyazanSpace.Domain.Auth.Services;
using RyazanSpace.Interfaces.Repositories;

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


            builder.Services.AddHttpClient<WebUserRepository>
                ( configureClient :
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/Users/"); });


            builder.Services.AddHttpClient<IRepository<EmailVerificationSession>, WebRepository<EmailVerificationSession>> 
                ( configureClient:
                client => { client.BaseAddress = new Uri($"{builder.Configuration["DatabaseAPI"]}/EmailVerificationSessions/"); });


            builder.Services.AddScoped(typeof(RegistrationService));
            builder.Services.AddScoped(typeof(EmailVerificationService));
        }
    }
}