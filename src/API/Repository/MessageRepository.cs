using API.Data;
using API.Models;
using API.Service;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MessageRepository : GenericRepository<Messages>, IMessageRepository
    {
        private readonly MessageDbContext _dbContext;

        private readonly IGenericRepository<TwilioCredentials> _TRepository;


        public MessageRepository(MessageDbContext dbContext, IGenericRepository<TwilioCredentials> TRepository)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _TRepository = TRepository;
        }
        public async Task<string> CreateAndSendMessage(Messages message)
        {

            try
            {

                // Get Twilio credentials
                var credentials = await _TRepository.Query().FirstOrDefaultAsync() ?? new TwilioCredentials();
                var Tmessage = await TwilioService.SendMessageAsync(credentials, message.Recipient, message.Message);

                if (string.IsNullOrEmpty(Tmessage.Sid))
                {
                    throw new Exception("Twilio credentials not found.");
                }

                // var createdMessage = await _MRepository.Create(message);

                //Use a transaction to ensure atomicity
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Insert into Messages table
                        var createdMessage = await Create(message);

                        // Insert into MessageSendings table
                        var messageSending = new MessageSending
                        {
                            MessageId = createdMessage.MessageId,
                            ConfirmationCode = Tmessage.Sid
                        };

                        _dbContext.Set<MessageSending>().Add(messageSending);
                        _dbContext.SaveChanges();
                        // Commit the transaction
                        await transaction.CommitAsync();

                        return Tmessage.Sid;
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction in case of error
                        await transaction.RollbackAsync();
                        throw; // Log or handle the exception as needed
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle global exceptions as needed
                throw new ApplicationException("An error occurred while processing the message.", ex);
            }
        }
    }
}
