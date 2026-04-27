namespace SVLaixe.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public int GroupId { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCritical { get; set; }
        public string Explanation { get; set; }

        public int ChapterId { get; set; }
    }
}
