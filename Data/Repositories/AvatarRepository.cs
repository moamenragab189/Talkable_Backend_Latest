
using Microsoft.EntityFrameworkCore;
using Talkable.Data.Entities;

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
            return await _context.Tb_Signs.Where(s => s.Name == word)
                .Select(s => s.AnimationPath).FirstOrDefaultAsync();
        }
    }
}