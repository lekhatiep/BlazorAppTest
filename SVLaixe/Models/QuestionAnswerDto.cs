namespace SVLaixe.Models
{
    public class QuestionAnswerDto
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public int GroupId { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public bool isCritical { get; set; }
        public string Explanation { get; set; }
        public List<Answer> Answers { get; set; }

        public int NumberAnswer { get; set; }
        
    }
}
