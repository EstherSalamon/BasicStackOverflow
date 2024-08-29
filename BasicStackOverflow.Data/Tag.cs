using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BasicStackOverflow.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QTJoining> JoinQuestions { get; set; }
    }
}
