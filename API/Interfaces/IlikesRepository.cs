using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IlikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicte, int userId);
    }
}
