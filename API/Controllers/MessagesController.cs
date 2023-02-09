using API.DTOs;
using API.Entities;
using API.Extensions;
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


    }
}
