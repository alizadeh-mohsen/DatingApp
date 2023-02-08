using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
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

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = dataContext.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = dataContext.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(l => l.SourceUserId == likesParams.UserId);
                users = likes.Select(l => l.TargetUser);
            }
            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(l => l.TargetUserId == likesParams.UserId);
                users = likes.Select(l => l.SourceUser);
            }


            var likdeUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                knownAs = user.KnownAs,
                Age = user.DateOfBirth.GetAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id,
            });
            return await PagedList<LikeDto>.CreateAsync(likdeUsers, likesParams.PageNumber, likesParams.PageSize);
        }
    }
}
