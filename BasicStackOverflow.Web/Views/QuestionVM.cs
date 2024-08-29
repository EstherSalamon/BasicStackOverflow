using BasicStackOverflow.Data;

namespace BasicStackOverflow.Web.Views
{
    public class QuestionVM
    {
        public string QuestionText { get; set; }
        public string Title { get; set; }
        public string[] Tags { get; set; }
        public int UserId { get; set; }
    }
}
