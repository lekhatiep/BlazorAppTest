using Microsoft.AspNetCore.Mvc;
using SVLaixe.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SVLaixe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        // GET: api/<QuestionsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<QuestionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var categories = await _questionRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost("AddQuestionFromJsonFile")]
        public async Task<IActionResult> AddQuestionFromJsonFile()
        {
            try
            {
                await _questionRepository.AddQuestionFromJsonFile();
                return Ok("Questions added successfully from JSON file.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddAnswerFollowQuestionIdFromJsonFile")]
        public async Task<IActionResult> AddAnswerFollowQuestionIdFromJsonFile()
        {
            try
            {
                await _questionRepository.AddAnswerFollowQuestionIdFromJsonFile();
                return Ok("Answers added successfully from JSON file.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("IsCorrectAnswerByQuestionId")]
        public async Task<IActionResult> IsCorrectAnswerByQuestionId(int questionId, int numberAnswer)
        {
            try
            {
                var isCorrect = await _questionRepository.IsCorrectAnswerByQuestionId(questionId, numberAnswer);
                return Ok(isCorrect ? 1 : 0);

            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");

            }
        }
        [HttpGet("GetQuestionAnswerByChapter")]
        public async Task<IActionResult> GetQuestionAnswerByCategoryId(int chapterID)
        {
            try
            {
                var questionAnswers = await _questionRepository.GetQuestionAnswerByChapterIdAsync(chapterID);
                return Ok(questionAnswers);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
