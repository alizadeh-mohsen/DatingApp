using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
  
    public class UsersController : BaseApiController
    {
        private readonly DataContext dataContext;

        public UsersController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await dataContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<AppUser> GetUser(int id)
        {
            return await dataContext.Users.FindAsync(id);
        }
    }
}