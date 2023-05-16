namespace RyazanSpace.DAL.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    }
}