using API.Models;

namespace API.Repository
{
    public interface IMessageRepository:IGenericRepository<Messages>
    {
        Task<string> CreateAndSendMessage(Messages message);
    }
}
