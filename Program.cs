using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Talkable.Data.Models;
using Talkable.Data.Repositories;
using Talkable.Hubs;
using Talkable.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddControllers();

// CORS عام (لـ API وغيره)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// ==== Dependency Injection المهم ====
builder.Services.AddDbContext<MainContext>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AvatarRepository>();
builder.Services.AddScoped<AvatarService>();
builder.Services.AddScoped<AnimationSeeder>();
builder.Services.AddSignalR();

// ===== Build App =====
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<AnimationSeeder>();
    await seeder.SeedAnimationsAsync();
}
// نفعّل CORS في البايبلاين
app.UseCors("AllowAll");

// ===== Static Files for Animations (GLB) مع CORS =====
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".glb"] = "model/gltf-binary";

// ===== حساب المسار بذكاء لمنع توقف البرنامج =====
var animationsPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Animations");

// إذا كان المسار يشير إلى مجلد الـ bin، نعود للخلف للوصول للمجلد الرئيسي للمشروع
if (animationsPath.Contains(@"bin\Debug") || !Directory.Exists(animationsPath))
{
    var baseDir = AppContext.BaseDirectory; // يبدأ من bin/Debug/net9.0
    var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..")); // يعود 3 مستويات للأعلى
    animationsPath = Path.Combine(projectRoot, "wwwroot", "Animations");
}

// أمان إضافي: لو المجلد مش موجود خالص، السيرفر هينشئه بنفسه ومش هيعمل Crash
if (!Directory.Exists(animationsPath))
{
    Directory.CreateDirectory(animationsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(animationsPath), // ✅ استخدام المسار الذكي هنا
    RequestPath = "/Animations",
    ContentTypeProvider = provider,
    ServeUnknownFileTypes = true,

    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    }
});
// باقي wwwroot (لو عندك css/js/صور أخرى)
app.UseStaticFiles();
app.MapHub<CallHub>("/callhub");
// Routing + Controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();