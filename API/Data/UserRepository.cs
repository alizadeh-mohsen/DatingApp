using API.DTOs;
using API.Entities;
using API.Helpers;
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
        private readonly ILogger<UserRepository> logger;

        public UserRepository(DataContext context, IMapper mapper, ILogger<UserRepository> logger)
        {
            dataContext = context;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<AppUser> GetUserById(int id)
        {
            return await dataContext.Users.FindAsync(id);
        }   
        
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await dataContext.Users
                 .Include(u => u.Photos)
                 .SingleOrDefaultAsync(c => c.UserName.ToLower() == username);
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

        public async Task<PagedList<MemberDto>> GetAllMembersAsync(UserParams userParams)
        {
            var query = dataContext.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            if (userParams.MaxAge > 0)
            {
                var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
                query = query.Where(u => u.DateOfBirth >= minDob);
            }
            if (userParams.MinAge > 0)
            {
                var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

                query = query.Where(u => u.DateOfBirth <= maxDob);
            }
            query = userParams.orderBy switch { 
            
                "created"=> query.OrderByDescending(u=>u.Created),
                _=> query.OrderByDescending(u=>u.LastActive),

            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }
    }
}
