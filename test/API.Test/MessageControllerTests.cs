using API.Controllers;
using API.DTO;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Test
{
    public class MessageControllerTests
    {

        private readonly Moq.Mock<IMessageRepository> _mockRepository;
        private readonly MessageController _controller;

        public MessageControllerTests()
        {
            _mockRepository = new Moq.Mock<IMessageRepository>();
            _controller = new MessageController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetFilterMessages_ShouldReturnOkResultWithMessages()
        {
            // Arrange: Set up a mock list of messages
            var messages = new List<Messages>
    {
        new Messages { MessageId = 1, Message = "Hello 1", Recipient = "+50377981234" },
        new Messages { MessageId = 2, Message = "Hello 2", Recipient = "+50377985678" }
    };

            // Set up the mock repository to return the messages asynchronously
            _mockRepository.Setup(rep => rep.GetAllMessages())
                .ReturnsAsync(messages);  // Mocking Task<IEnumerable<Messages>>

            var controller = new MessageController(_mockRepository.Object);

            // Act: Call the method under test (remember to await the async method)
            var result = await controller.GetMessages();  // Use 'await' here to handle async execution

            // Assert: Verify the result is OkObjectResult and contains the expected list
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure the result is OK (200)
            var returnedMessages = Assert.IsType<List<MessageDTO>>(okResult.Value); // Ensure the returned value is a list of DTOs
            Assert.Equal(2, returnedMessages.Count); // Verify that the list contains the expected number of messages


        }



        [Fact]
        public async Task CreateMessage_ValidMessage_ShouldReturnOkResultWithTwilioId()
        {
            // Arrange
            var messageDTO = new MessageDTO
            {
                MessageId = 1,
                Message = "Test message",
                Recipient = "+123456789"
            };

            var message = new Messages
            {
                MessageId = 1,
                Message = "Test message",
                Recipient = "+123456789"
            };

            _mockRepository.Setup(repo => repo.CreateAndSendMessage(It.IsAny<Messages>()))
                 .ReturnsAsync("Twilo");

            // Act
            var result = await _controller.CreateMessage(messageDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Access the 'Twilo' property in  object returned by Ok()
            var returnedValue = ((TwiloResponse)okResult.Value).Twilo;
            Assert.Equal("Twilo", returnedValue);  // Verify the TwilioGeneratedId
        }



    }
}
