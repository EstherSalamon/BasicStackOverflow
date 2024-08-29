using System.ComponentModel.DataAnnotations.Schema;

namespace BasicStackOverflow.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string AnswerText { get; set; }
        public DateTime DatePosted { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}
