
using System.Reflection.Metadata.Ecma335;
using Talkable.Data.Repositories;

namespace Talkable.Services
{
    public class AvatarService
    {
        private readonly AvatarRepository _AvatarRepository;
        public AvatarService(AvatarRepository avatarRepository)
        {
            _AvatarRepository = avatarRepository;
        }
        public async Task<string?> GetAction(string word)
        {
            return await _AvatarRepository.GetAction(word);
        }

    }
}