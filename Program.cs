
using Talkable.Data.Models;
using Talkable.Data.Repositories;
using Talkable.Services;

namespace Talkable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<MainContext>();
            builder.Services.AddScoped<AuthRepository>();
            builder.Services.AddScoped<AuthService>();
            //=======================================
            builder.Services.AddScoped<AvatarService>();
            builder.Services.AddScoped<AvatarRepository>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()           // أو حدد origins معينة في الإنتاج
                        .AllowAnyMethod()           // ده مهم جدًا عشان يشمل OPTIONS + POST + ...
                        .AllowAnyHeader()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // اختياري بس مفيد
                });
            });
            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
