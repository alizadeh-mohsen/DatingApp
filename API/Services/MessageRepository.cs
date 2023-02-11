using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public MessageRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }
        public async void DeleteMessage(Message message)
        {
            dataContext.Messages.Remove(message);
        }
        public async Task<bool> SaveAllChanges()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }
        public void AddMessage(Message message)
        {
            dataContext.Messages.Add(message);
        }
        public async Task<Message> GetMessage(int id)
        {
            return await dataContext.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThread(string senderUsername, string recepientUsername)
        {
            var messages = await dataContext.Messages
                .Include(m => m.Sender).ThenInclude(m => m.Photos)
                .Include(m => m.Recipient).ThenInclude(m => m.Photos).
                Where(
                m => m.Sender.UserName == senderUsername && m.RecipientDeleted==false &&
                m.Recipient.UserName == recepientUsername ||
                m.Sender.UserName == recepientUsername && m.SenderDeleted==false &&
                m.Recipient.UserName == senderUsername)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            var unreadMessage = dataContext.Messages.
                Where(m => m.DateRead == null && m.RecipientUsername == senderUsername);

            if (unreadMessage.Any())
            {

                foreach (var message in unreadMessage)
                {
                    message.DateRead = DateTime.Now;
                }
                await dataContext.SaveChangesAsync();
            }
            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = dataContext.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();

            query = messageParams.Container.ToLower() switch
            {
                "inbox" => query.Where(m => m.Recipient.UserName == messageParams.Username && m.RecipientDeleted == false),
                "outbox" => query.Where(m => m.Sender.UserName == messageParams.Username && m.SenderDeleted == false),
                _ => query.Where(m => m.Recipient.UserName == messageParams.Username && m.DateRead == null && m.RecipientDeleted == false),
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }
    }

}