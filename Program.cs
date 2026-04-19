using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talkable.Data.Models;
using Talkable.Data.Repositories;
using Talkable.Hubs;
using Talkable.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// ==== Dependency Injection  ====
builder.Services.AddDbContext<MainContext>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AvatarRepository>();
builder.Services.AddScoped<AvatarService>();
builder.Services.AddScoped<AnimationSeeder>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddSignalR();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
builder.Services.AddAuthorization();
// ===== Build App =====
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<AnimationSeeder>();
    await seeder.SeedAnimationsAsync();
}
app.UseCors("AllowAll");

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".glb"] = "model/gltf-binary";


var animationsPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Animations");


if (animationsPath.Contains(@"bin\Debug") || !Directory.Exists(animationsPath))
{
    var baseDir = AppContext.BaseDirectory; 
    var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..")); 
    animationsPath = Path.Combine(projectRoot, "wwwroot", "Animations");
}


if (!Directory.Exists(animationsPath))
{
    Directory.CreateDirectory(animationsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(animationsPath), 
    RequestPath = "/Animations",
    ContentTypeProvider = provider,
    ServeUnknownFileTypes = true,

    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    }
});

app.UseStaticFiles();
app.MapHub<CallHub>("/callhub");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();