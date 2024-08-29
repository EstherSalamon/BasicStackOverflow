using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BasicStackOverflow.Data
{
    public class QTJoining
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Question Question { get; set; }
        public Tag Tag { get; set; }
    }
}
