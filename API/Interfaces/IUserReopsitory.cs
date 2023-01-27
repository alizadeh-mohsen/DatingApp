﻿using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserReopsitory
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<MemberDto> GetMemberAsync(string username);
        Task<IEnumerable<MemberDto>> GetAllMembersAsync();
    }
}
