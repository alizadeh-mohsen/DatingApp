using API.Data;
using API.Entities;
using API.Helpers;
using API.Interfaces;

namespace API.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext dataContext;

        public MessageRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async void DeleteMessage(int messageId)
        {
            dataContext.Messages.Remove(message);
        }
        public async Task<bool> SaveAllChanges()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }
        public void SendMessage(Message message)
        {
            dataContext.Messages.Add(message);
        }
        public async Task<Message> GetMessage(int id)
        {
            return await dataContext.Messages.FindAsync(id);
        }

        public async Task<PagedList<Message>> GetMessagesThread(int senderId, int receiverId)
        {
            var query = dataContext.Messages.Where(m => m.SenderId == senderId && m.RecipientDeleted == receiverId).AsQueryable();
        }

        public Task<PagedList<Message>> GetSentMessagesOfUser()
        {
            throw new NotImplementedException();
        }

       
    }
}
