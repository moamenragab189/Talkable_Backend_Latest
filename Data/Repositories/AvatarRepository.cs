
using Microsoft.EntityFrameworkCore;
using Talkable.Data.Models;

namespace Talkable.Data.Repositories
{
    public class AvatarRepository
    {
            private readonly MainContext _context;
            public AvatarRepository(MainContext context)
            {
                _context = context;
            }
        public async Task<string?> GetAction(string word)
        {
            return await _context.Tb_Signs.Where(s => s.name_ar == word)
                .Select(s => s.Animation_path).FirstOrDefaultAsync();
        }
    }
}
