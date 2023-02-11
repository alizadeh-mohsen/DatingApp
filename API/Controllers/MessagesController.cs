using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMessageRepository messageRepository;
        private readonly IUserReopsitory userReopsitory;
        private readonly IMapper mapper;

        public MessagesController(IMessageRepository messageRepository, IUserReopsitory userReopsitory, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.userReopsitory = userReopsitory;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageDto)
        {
            var username = User.GetUsername();
            if (username == null)
                return BadRequest("Logged in user is null");

            if (username == messageDto.RecipientUsername) return BadRequest("You cannot send message to yourself");

            var sender = await userReopsitory.GetUserByUsernameAsync(username);
            var recipiant = await userReopsitory.GetUserByUsernameAsync(messageDto.RecipientUsername);
            if (recipiant == null) return BadRequest();

            var message = new Message
            {
                Content = messageDto.Content,
                MessageSent = DateTime.Now,
                Sender = sender,
                Recipient = recipiant,
                RecipientUsername = messageDto.RecipientUsername,
                SenderUsername = username
            };

            messageRepository.AddMessage(message);
            if (await messageRepository.SaveAllChanges()) return Ok(mapper.Map<MessageDto>(message));

            return BadRequest("Message not sent");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await messageRepository.GetMessagesForUser(messageParams);
            PaginationHeader paginationHeader = new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            Response.AddPaginationHeader(paginationHeader);

            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<IEnumerable<MessageDto>> GetMessagesThread(string username)
        {
            var currentUsername = User.GetUsername();
            return await messageRepository.GetMessagesThread(currentUsername, username);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await messageRepository.GetMessage(id);
            if (message == null)
                return NotFound();

            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();

            if (message.SenderUsername == username) message.SenderDeleted = true;
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                messageRepository.DeleteMessage(message);
            }
            if (await messageRepository.SaveAllChanges())
                return Ok();

            return BadRequest("Cannot delete message");
        }

    }
}
