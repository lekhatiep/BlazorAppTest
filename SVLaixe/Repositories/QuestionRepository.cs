using Dapper;
using Microsoft.Data.SqlClient;
using SVLaixe.Models;

namespace SVLaixe.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IConfiguration _configuration;

        public QuestionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            var connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                var questions = await connection.QueryAsync<Question>("SELECT Id, QuestionNumber, Content FROM Questions");
                return questions.ToList();
            }
        }

        async Task<List<Category>> IQuestionRepository.GetCategoriesAsync()
        {
            var connectionString =  GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                var categories = await connection.QueryAsync<Category>("SELECT Id, Name, Description FROM Categories");
                return categories.ToList();
            }
        }


        public async Task<List<QuestionAnswerDto>> GetQuestionsByCategoryIdAsync(int categoryId)
        {
            var connectionString = GetConnectionString();
            var questionAnswerList = new List<QuestionAnswerDto>();
            using (var connection = new SqlConnection(connectionString))
            {
                var questions = await connection.QueryAsync<Question>("SELECT * FROM Questions q INNER JOIN Chapter ch ON ch.ChapterID = Q.ChapterID WHERE ch.CatID = @Category", new { Category = categoryId });
                foreach (var question in questions)
                {
                    var answers = await connection.QueryAsync<Answer>(
                        "SELECT Id, QuestionId, Content, IsCorrect, NumberAnswer FROM Answers WHERE QuestionId = @QuestionNumber",
                        new { QuestionNumber = question.QuestionNumber });
                    var questionAnswerDto = new QuestionAnswerDto
                    {
                        Id = question.Id,
                        QuestionNumber = question.QuestionNumber,
                        Content = question.Content,
                        Answers = answers.ToList(),
                        ChapterId = question.ChapterId,
                        IsCritical = question.IsCritical,
                        Explanation = question.Explanation,
                    };
                    questionAnswerList.Add(questionAnswerDto);
                }
                return questionAnswerList;
            }
        }

        public async Task AddQuestionFromJsonFile()
        {
            var jsonFile = Directory.GetCurrentDirectory() + @"/wwwroot/Data/questions_full.json";
            if (!File.Exists(jsonFile))
            {
                throw new FileNotFoundException("The questions.json file was not found.");
            }

            var jsonData = await File.ReadAllTextAsync(jsonFile);
            var questions = System.Text.Json.JsonSerializer.Deserialize<List<QuestionListDto>>(jsonData);
            if (questions == null || questions.Count == 0)
            {
                throw new Exception("No questions found in the JSON file.");
            }

            var connectionString = GetConnectionString();
            var listQuestions = new List<Question>();
            foreach (var questionDto in questions)
            {
                var question = new Question
                {
                    QuestionNumber = questionDto.id,
                    Content = questionDto.question,
                };
                listQuestions.Add(question);
            }

            using (var connection = new SqlConnection(connectionString))
            {
                foreach (var question in listQuestions)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO Questions (QuestionNumber, Content) VALUES (@QuestionNumber, @Content)",
                        new { QuestionNumber = question.QuestionNumber, Content = question.Content });
                }
            }
        }
        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task AddAnswerFollowQuestionIdFromJsonFile()
        {
            var fileJson = Directory.GetCurrentDirectory() + @"/wwwroot/Data/questions_full.json";
            if (!File.Exists(fileJson))
            {
                throw new FileNotFoundException("The answers.json file was not found.");
            }
            var jsonData = File.ReadAllText(fileJson);
            var questions = System.Text.Json.JsonSerializer.Deserialize<List<QuestionListDto>>(jsonData);
            var connectionString = GetConnectionString();

            if (questions == null || questions.Count == 0)
            {
                throw new Exception("No questions found in the JSON file.");
            }

            foreach (var questionDto in questions)
            {
                var questionId = questionDto.id;
                var answers = questionDto.answers;
                var correctAnswerIndex = questionDto.correct;
                using (var connection = new SqlConnection(connectionString))
                {
                    for (int i = 0; i < answers.Count; i++)
                    {
                        var numberAnswer = i + 1;
                        var answerText = answers[i];
                        var isCorrect = (i == correctAnswerIndex - 1);
                        await connection.ExecuteAsync(
                            "INSERT INTO Answers (QuestionId, Content, IsCorrect, NumberAnswer) VALUES (@QuestionId, @Content, @IsCorrect, @NumberAnswer)",
                            new { QuestionId = questionId, Content = answerText, IsCorrect = isCorrect, NumberAnswer = numberAnswer });
                    }
                }
            }
        }

        public async Task<bool> IsCorrectAnswerByQuestionId(int questionId, int numberAnswer)
        {
            var connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                var isCorrect =  await connection.QueryFirstOrDefaultAsync<bool>(
                    "SELECT IsCorrect FROM Answers WHERE QuestionId = @QuestionId AND NumberAnswer = @NumberAnswer",
                    new { QuestionId = questionId, NumberAnswer = numberAnswer });
                return isCorrect;
            }
        }

        public async Task<List<QuestionAnswerDto>> GetQuestionAnswerByChapterIdAsync(int ChapterId)
        {
            var connectionString = GetConnectionString();
            var questionAnswerList = new List<QuestionAnswerDto>();
            using (var connection = new SqlConnection(connectionString))
            {
                var questions = await connection.QueryAsync<Question>("SELECT Id, QuestionNumber, Content FROM Questions WHERE ChapterId = @ChapterId", new { ChapterId = ChapterId });
                
                foreach (var question in questions)
                {
                    var answers = await connection.QueryAsync<Answer>(
                        "SELECT Id, QuestionId, Content, IsCorrect, NumberAnswer FROM Answers WHERE QuestionId = @QuestionId",
                        new { QuestionId = question.Id });
                    var questionAnswerDto = new QuestionAnswerDto
                    {
                        Id = question.Id,
                        QuestionNumber = question.QuestionNumber,
                        Content = question.Content,
                        Answers = answers.ToList()
                    };
                    questionAnswerList.Add(questionAnswerDto);
                }
                return questionAnswerList;
            }
        }

        public async Task<List<QuestionAnswerDto>> GetRandomExampleQuestionB()
        {
            var connectionString = GetConnectionString();
            var questionAnswerList = new List<QuestionAnswerDto>();
            using (var connection = new SqlConnection(connectionString))
            {
                var questions = await connection.QueryAsync<Question>("[dbo].[GetRandomExamQuestion_CatB]", commandType: System.Data.CommandType.StoredProcedure);
                foreach (var question in questions)
                {
                    var answers = await connection.QueryAsync<Answer>(
                        "SELECT Id, QuestionId, Content, IsCorrect, NumberAnswer FROM Answers WHERE QuestionId = @QuestionId",
                        new { QuestionId = question.Id });
                    var questionAnswerDto = new QuestionAnswerDto
                    {
                        Id = question.Id,
                        QuestionNumber = question.QuestionNumber,
                        Content = question.Content,
                        Answers = answers.ToList()
                    };
                    questionAnswerList.Add(questionAnswerDto);
                }
                return questionAnswerList;
            }
        }

        public async Task AddExplainFollowQuestionIdFromJsonFile()
        {
            var fileJson = Directory.GetCurrentDirectory() + @"/wwwroot/Data/600_cau_explain.json";
            if (!File.Exists(fileJson))
            {
                throw new FileNotFoundException("The explanations.json file was not found.");
            }
            var jsonData = File.ReadAllText(fileJson);
            var questions = System.Text.Json.JsonSerializer.Deserialize<List<QuestionExplainDto>>(jsonData);
            if (questions == null || questions.Count == 0)
            {
                throw new Exception("No questions found in the JSON file.");
            }

            var connectionString = GetConnectionString();

            foreach (var questionDto in questions)
            {
                var questionId = questionDto.number;
                var explanation = questionDto.explanation;
             
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(
                        "UPDATE Questions SET Explanation = @Explanation WHERE QuestionNumber = @QuestionNumber",
                        new { Explanation = explanation, QuestionNumber = questionId });
                }

            }
        }
    }
}
