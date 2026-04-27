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

    public class QuestionExplainDto
    {
        public int number { get; set; }
        public string question { get; set; }
        public string category { get; set; }
        public List<Answer> answers { get; set; }
        public string explanation { get; set; }
        public string hinhanhq { get; set; }
        public object hinhanhqAlt { get; set; }
    }
}
