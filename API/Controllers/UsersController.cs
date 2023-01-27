using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserReopsitory userReopsitory;

        public UsersController(IUserReopsitory reopsitory)
        {
            userReopsitory = reopsitory;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await userReopsitory.GetAllUsersAsync();
        }

        [HttpGet("{username}")]
        
        public async Task<AppUser> GetUser(string username)
        {
            return await userReopsitory.GetUserByUsernameAsync(username);
        }
    }
}