
using Talkable.Data.Models;
using Talkable.Data.Repositories;
using Talkable.Services;

namespace Talkable
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<MainContext>();
            builder.Services.AddScoped<AuthRepository>();
            builder.Services.AddScoped<AuthService>();

            // add seeder service animation file work (Mg13)
            builder.Services.AddScoped<AnimationSeeder>();

            var app = builder.Build();

            // to make seeder animation file work (Mg13)
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<AnimationSeeder>();
                await seeder.SeedAnimationsAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // to use wwwroot folder (Mg13)
            app.UseStaticFiles();

            app.UseAuthorization();
            

            app.MapControllers();



            app.Run();
        }
    }
}
