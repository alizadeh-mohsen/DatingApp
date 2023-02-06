using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
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
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var user = await userReopsitory.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;

            userParams.Gender = userParams.Gender;

            var users = await userReopsitory.GetAllMembersAsync(userParams);
            PaginationHeader paginationHeader = new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            Response.AddPaginationHeader(paginationHeader);
            return Ok(users);
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
        [HttpDelete("delete-photo/{photoIdregs}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await userReopsitory.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You cannot delete main photo");

            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);

            user.Photos.Remove(photo);

            if (await userReopsitory.SaveAllAsync())
                return Ok();

            return BadRequest("Photo not uploaded");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            AppUser user = await userReopsitory.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null) return NotFound();

            var mainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

            if (mainPhoto != null)
            {
                if (mainPhoto.Id == photoId)
                    return NoContent();

                mainPhoto.IsMain = false;
            }

            photo.IsMain = true;
            if (await userReopsitory.SaveAllAsync()) return NoContent();
            return BadRequest("Problem setting the main photo");
        }
    }
}