using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        private readonly IPhotoService photoService;

        public UsersController(IUserReopsitory reopsitory, IMapper mapper, IPhotoService photoService)
        {
            userReopsitory = reopsitory;
            this.mapper = mapper;
            this.photoService = photoService;
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
            var username = User.GetUsername();

            var user = await userReopsitory.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            mapper.Map(memberUpdateDto, user);
            if (await userReopsitory.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();
            var user = await userReopsitory.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (!user.Photos.Any())
                photo.IsMain = true;
            user.Photos.Add(photo);

            if (await userReopsitory.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser),
                    new { username = user.UserName },
                    mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Photo not uploaded");
        }
    }
}