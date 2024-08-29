using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasicStackOverflow.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string QuestionText { get; set; }
        public DateTime DatePosted { get; set; }
        public List<QTJoining> JoinTags { get; set; }
        public List<Answer> Answers { get; set; }
        public int UserId { get; set; }
        [NotMapped]
        public User User { get; set; }
    }
}
