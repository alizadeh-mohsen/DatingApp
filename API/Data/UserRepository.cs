﻿using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserReopsitory
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            dataContext = context;
            this.mapper = mapper;
        }
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
           return await dataContext.Users.SingleOrDefaultAsync(c => c.UserName.ToLower() == username);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await dataContext.Users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            dataContext.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await dataContext.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        }

        public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
        {
            return await dataContext.Users
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
