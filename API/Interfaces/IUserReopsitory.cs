using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserReopsitory
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<MemberDto>> GetAllUsersAsync();
        Task<MemberDto> GetUserByUsernameAsync(string username);

        Task<MemberDto> GetMemberAsync(string username);
        Task<IEnumerable<MemberDto>> GetAllMembersAsync();
    }
}
