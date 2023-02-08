using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class LikeRepository : IlikesRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public LikeRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await dataContext.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await dataContext.Users.Include(c => c.LikedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var users = dataContext.Users.OrderBy(u=>u.UserName).AsQueryable();
            var likes=dataContext.Likes.AsQueryable();

            if (predicate == "liked")
            {
                likes = likes.Where(l => l.SourceUserId == userId);
                users = likes.Select(l => l.TargetUser); 
            }   if (predicate == "likedBy")
            {
                likes = likes.Where(l => l.TargetUserId== userId);
                users = likes.Select(l => l.SourceUser); 
            }

            
            return await users.Select(user=>new LikeDto
            {
                UserName=user.UserName,
                knownAs=user.KnownAs,
                Age=user.DateOfBirth.GetAge(),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain).Url,
                City=user.City,
                Id=user.Id,
            }).ToListAsync();
        }
    }
}
