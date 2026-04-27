using Microsoft.AspNetCore.Mvc;
using Moq;
using SVLaixe.Controllers;
using SVLaixe.Models;
using SVLaixe.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace LaixeUnitTests
{
    public class QuestionControllerTests
    {
        [Fact]
        public async Task GetQuestionAnswerByChapterID_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();

            var fakeData = new List<QuestionAnswerDto>
            {
                new QuestionAnswerDto { Id = 2 }
            };

            mockRepo.Setup(x => x.GetQuestionAnswerByChapterIdAsync(1))
                    .ReturnsAsync(fakeData);

            var controller = new QuestionsController(mockRepo.Object);

            // Act
            var result = await controller.GetQuestionAnswerByChapterID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<QuestionAnswerDto>>(okResult.Value);

            Assert.NotEmpty(data);
        }
        [Fact]
        public async Task GetQuestionAnswerByChapterID_ReturnsBadRequest_WhenException()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();

            mockRepo.Setup(x=> x.GetQuestionAnswerByChapterIdAsync(It.IsAny<int>())).ThrowsAsync(new System.Exception("Database error"));
            var controller = new SVLaixe.Controllers.QuestionsController(mockRepo.Object);
            // Act
            var result = await controller.GetQuestionAnswerByChapterID(1);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred: Database error", badRequestResult.Value.ToString());
        }
    }
}
