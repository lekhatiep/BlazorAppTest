using SVLaixe.Models;

namespace SVLaixe.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<List<Question>> GetQuestionsByCategoryIdAsync(int categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task AddQuestionFromJsonFile();

        Task AddAnswerFollowQuestionIdFromJsonFile();

        Task<bool> IsCorrectAnswerByQuestionId(int questionId, int numberAnswer);
        Task<List<QuestionAnswerDto>> GetQuestionAnswerByChapterIdAsync(int chapterID);
    }
}
