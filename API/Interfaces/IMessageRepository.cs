using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void SendMessage(Message message);

        void DeleteMessage(int messageId);

        Task<Message> GetMessage(int id); 
        Task<PagedList<Message>> GetMessagesThread(int senderId,int receiverId);
        Task<PagedList<Message>> GetSentMessagesOfUser();
        Task<bool> SaveAllChanges();
        

    }
}
