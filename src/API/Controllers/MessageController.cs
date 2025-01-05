using API.Models;
using API.Repository;
using API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository _repository;
        //private readonly IGenericRepository<TwilioCredentials> _Trepository;
        //private readonly IGenericRepository<MessageSending> _Mrepository;


        //public MessageController(IGenericRepository<Messages> repository, IGenericRepository<TwilioCredentials> Trepository, IGenericRepository<MessageSending> Mrepository)
        //{
        //    _repository = repository;
        //    _Trepository = Trepository;
        //    _Mrepository = Mrepository;
        //}


      
        public MessageController(IMessageRepository repository)
        {
            _repository = repository;
        }
        // GET: api/messages
        [HttpGet]
        public async Task<IActionResult> GetFilterMessages()
        {
            var messages = await _repository.Query().ToListAsync();
            return Ok(messages);
        }

        // GET: api/messages/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var message = await _repository.GetModel(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        // POST: api/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage( Messages message)
        {
            if (message == null)
            {
                return BadRequest();
            }
         

                     var Twilo = await _repository.CreateAndSendMessage(message);
            // CreatedAtAction(nameof(GetMessage), new { id = createdMessage.MessageId }, createdMessage);
            return Ok(new { Twilo });
        }

        // PUT: api/messages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, Messages message)
        {
            if (message == null || message.MessageId != id)
            {
                return BadRequest();
            }

            var existingMessage = await _repository.GetModel(m => m.MessageId == id);
            existingMessage.Message = message.Message;
            if (existingMessage == null)
            {
                return NotFound();
            }

            await _repository.Edit(existingMessage);
            return NoContent();
        }

        // DELETE: api/messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _repository.GetModel(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            await _repository.Delete(message);
            return NoContent();
        }
    }
}
