using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserReopsitory
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        public Task<AppUser> GetUserById(int id);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<MemberDto> GetMemberAsync(string username);
        Task<PagedList<MemberDto>> GetAllMembersAsync(UserParams userParams);
    }
}
