using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUserReopsitory userReopsitory;
        private readonly IlikesRepository likesRepository;

        public LikesController(IUserReopsitory userReopsitory, IlikesRepository ilikesRepository)
        {
            this.userReopsitory = userReopsitory;
            this.likesRepository = ilikesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var targetUser = await userReopsitory.GetUserByUsernameAsync(username);
            var sourceUser = await likesRepository.GetUserWithLikes(sourceUserId);

            if (targetUser == null) return BadRequest("User not found");

            if (targetUser.UserName == username) return BadRequest("You cannot like yoursefl");


            if (await likesRepository.GetUserLike(sourceUserId, targetUser.Id) != null)
                return BadRequest("You already like this person");

            sourceUser.LikedUsers.Add(new Entities.UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUser.Id
            });

            if (await userReopsitory.SaveAllAsync())
                return Ok();

            return BadRequest("Something went wrong");
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikes(string predciate)
        {
            var users = await likesRepository.GetUserLikes(predciate, User.GetUserId());

            return Ok(users);
        }


    }
}
