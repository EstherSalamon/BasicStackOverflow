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
            List<Question> q = repo.GetAll();
            string json = JsonConvert.SerializeObject(q, new JsonSerializerSettings
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
            Question q = repo.GetById(id);

            if(q == null)
            {
                return null;
            }

            UserRepository uRepo = new UserRepository(_connection);

            q.User = uRepo.GetById(q.UserId);

            foreach(var a in q.Answers)
            {
                a.User = uRepo.GetById(a.UserId);
            }

            string json = JsonConvert.SerializeObject(q, new JsonSerializerSettings
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
    }
}
