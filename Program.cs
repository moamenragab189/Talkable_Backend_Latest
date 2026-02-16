using Microsoft.AspNetCore.StaticFiles;
using Talkable.Data.Models;
using Talkable.Data.Repositories;
using Talkable.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// ==== Dependency Injection المهم ====
// لازم كل service/ repository اللي controllers محتاجاها
builder.Services.AddDbContext<MainContext>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>(); 
builder.Services.AddScoped<AvatarRepository>();
builder.Services.AddScoped<AvatarService>();
// add seeder service animation file work (Mg13)
 builder.Services.AddScoped<AnimationSeeder>();


// ===== Build App =====
var app = builder.Build();

// CORS
app.UseCors("AllowAll");

// ===== Static Files for Animations =====
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".glb"] = "model/gltf-binary";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Animations")
    ),
    RequestPath = "/Animations",
    ContentTypeProvider = provider,
    ServeUnknownFileTypes = true
});

// باقي wwwroot
app.UseStaticFiles();

// Routing + Controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
