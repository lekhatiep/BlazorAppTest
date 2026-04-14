namespace SVLaixe.Models
{
    public class QuestionListDto
    {
        public int id { get; set; }
        public string question { get; set; }
        public List<string> answers { get; set; }
        public int correct { get; set; }
        public string correct_answer_text { get; set; }
        public List<object> listImages { get; set; }
        public object imageName { get; set; }
    }
}
