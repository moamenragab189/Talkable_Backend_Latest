// --------------------- Mg13 ---------------------------


//using Talkable.Data.Models;
//namespace Talkable.Services
//{
//    public class AnimationSeeder
//    {
//        private readonly MainContext _context;
//        private readonly IWebHostEnvironment _env;

//        public AnimationSeeder(MainContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;
//        }

//        public async Task SeedAnimationsAsync()
//        {
//            var path = Path.Combine(_env.WebRootPath, "Animations");

//            if (!Directory.Exists(path))
//                return;

//            var files = Directory.GetFiles(path, "*.glb");

//            var existingSigns = _context.Tb_Signs
//                .Select(s => s.Name)
//                .ToHashSet();

//            foreach (var file in files)
//            {
//                var fileName = Path.GetFileNameWithoutExtension(file);

//                if (!existingSigns.Contains(fileName))
//                {
//                    _context.Tb_Signs.Add(new Signs
//                    {
//                        Name = fileName,
//                        AnimationPath = $"/Animations/{fileName}.glb"
//                    });
//                }
//            }

//            await _context.SaveChangesAsync();
//        }
//    }
//}


// --------------------- Mg13 ---------------------------

using Talkable.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Talkable.Data.Entities; // للتأكد من قراءة المكتبة بشكل صحيح

namespace Talkable.Services
{
    public class AnimationSeeder
    {
        private readonly MainContext _context;
        private readonly IWebHostEnvironment _env;

        public AnimationSeeder(MainContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task SeedAnimationsAsync()
        {
            //// ✅ تم التعديل هنا: نستخدم ContentRootPath ونوجهه مباشرة لمجلد wwwroot/Animations
            //var path = Path.Combine(_env.ContentRootPath, "wwwroot", "Animations");

            //if (!Directory.Exists(path))
            //    return;

            //var files = Directory.GetFiles(path, "*.glb");

            //var existingSigns = 
            //    _context.Tb_Signs
            //    .Select(s => s.Name)
            //    .ToHashSet();

            //foreach (var file in files)
            //{
            //    var fileName = Path.GetFileNameWithoutExtension(file);

            //    if (!existingSigns.Contains(fileName))
            //    {
            //        _context.Tb_Signs.Add(new Signs
            //        {
            //            Name = fileName,
            //            AnimationPath = $"/Animations/{fileName}.glb"
            //        });
            //    }
            //}

            await _context.SaveChangesAsync();
        }
    }
}