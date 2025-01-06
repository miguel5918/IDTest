using API.DTO;
using API.Extensions;
using API.Models;
using API.Repository;
using API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio.TwiML.Messaging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository _repository;
      
      
        public MessageController(IMessageRepository repository)
        {
            _repository = repository;
        }
        // GET: api/messages
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _repository.GetAllMessages();          
            var messagesDTO = Mapper.MapList<API.Models.Messages, MessageDTO>(messages);
            return Ok(messagesDTO);
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
            // CLASS FROM EF TO DTO
            var messageDTO = Mapper.Map<API.Models.Messages, MessageDTO>(message);

            return Ok(messageDTO);
        }

        // POST: api/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageDTO messageDTO)
        {
            if (messageDTO == null)
            {
                return BadRequest();
            }

            // CONVERT DTO TO INSERT CLASS
            var Message = Mapper.Map<API.DTO.MessageDTO, API.Models.Messages>(messageDTO);
                     var Twilo = await _repository.CreateAndSendMessage(Message);
           
            return Ok(new TwiloResponse(){ Twilo =Twilo });
        }

        // PUT: api/messages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, MessageDTO messageDTO)
        {
            if (messageDTO == null || messageDTO.MessageId != id)
            {
                return BadRequest();
            }
            // CONVERT DTO TO UPDATE CLASS
            var Message = Mapper.Map<API.DTO.MessageDTO, API.Models.Messages>(messageDTO);
            var existingMessage = await _repository.GetModel(m => m.MessageId == id);
            existingMessage.Message = Message.Message;
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
