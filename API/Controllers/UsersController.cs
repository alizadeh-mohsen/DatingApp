using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserReopsitory userReopsitory;
        private readonly IMapper mapper;

        public UsersController(IUserReopsitory reopsitory, IMapper mapper)
        {
            userReopsitory = reopsitory;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MemberDto>> GetUsers()
        {
            return await userReopsitory.GetAllMembersAsync();
        }

        [HttpGet("{username}")]

        public async Task<MemberDto> GetUser(string username)
        {
            return await userReopsitory.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user =await userReopsitory.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            mapper.Map(memberUpdateDto, user);
            if (await userReopsitory.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");

        }
    }
}