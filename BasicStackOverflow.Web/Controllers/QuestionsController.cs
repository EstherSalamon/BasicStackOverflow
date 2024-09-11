using BasicStackOverflow.Data;
using BasicStackOverflow.Web.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;

namespace BasicStackOverflow.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly string _connection;

        public QuestionsController(IConfiguration config)
        {
            _connection = config.GetConnectionString("ConStr");
        }

        [HttpGet("getall")]
        public string GetAllQuestions()
        {
            QARepository repo = new QARepository(_connection);
            List<Question> questions = repo.GetAll();
            string json = JsonConvert.SerializeObject(questions, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return json;
        }

        [HttpPost("add")]
        [Authorize]
        public void AddQuestion(QuestionVM vm)
        {
            Question q = new Question
            {
                Title = vm.Title,
                QuestionText = vm.QuestionText,
                UserId = vm.UserId,
                DatePosted = DateTime.Now
            };

            List<string> tags = vm.Tags.ToList();
            QARepository repo = new QARepository(_connection);
            repo.AddQuestion(q, tags);
        }

        [HttpGet("byid")]
        public string GetById(int id)
        {
            QARepository repo = new QARepository(_connection);
            Question question = repo.GetById(id);

            if(question == null)
            {
                return null;
            }

            UserRepository uRepo = new UserRepository(_connection);

            question.User = uRepo.GetById(question.UserId);

            foreach(var a in question.Answers)
            {
                a.User = uRepo.GetById(a.UserId);
            }

            string json = JsonConvert.SerializeObject(question, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

        [HttpPost("answer")]
        public void AddAnswer(AnswerVM vm)
        {
            vm.Answer.DatePosted = DateTime.Now;
            QARepository repo = new QARepository(_connection);
            repo.AddAnswer(vm.Answer);
        }

        [HttpGet("searchbytag")]
        public string GetQuestionsByTag(int tagId)
        {
            QARepository repo = new QARepository(_connection);
            List<Tag> tagList = repo.GetQuestionsByTag(tagId);

            List<Question> disassembledTagList = new List<Question>();
            foreach(var l in tagList)
            {
                List<QTJoining> joinQ = l.JoinQuestions;
                foreach(var jq in joinQ)
                {
                    jq.Question.Answers = repo.GetAnswersById(jq.QuestionId);
                    disassembledTagList.Add(jq.Question);
                }
            }

            string json = JsonConvert.SerializeObject(disassembledTagList, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return json;
        }

        [HttpGet("gettags")]
        public List<Tag> GetTags()
        {
            QARepository repo = new QARepository(_connection);
            return repo.GetAllTags();
        }
    }
}
