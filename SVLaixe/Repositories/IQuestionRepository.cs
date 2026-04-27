using SVLaixe.Models;

namespace SVLaixe.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<List<QuestionAnswerDto>> GetQuestionsByCategoryIdAsync(int categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task AddQuestionFromJsonFile();

        Task AddAnswerFollowQuestionIdFromJsonFile();
        Task AddExplainFollowQuestionIdFromJsonFile();

        Task<bool> IsCorrectAnswerByQuestionId(int questionId, int numberAnswer);
        Task<List<QuestionAnswerDto>> GetQuestionAnswerByChapterIdAsync(int chapterID);
        Task<List<QuestionAnswerDto>> GetRandomExampleQuestionB();
    }
}
