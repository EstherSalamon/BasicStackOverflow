using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BasicStackOverflow.Data
{
    public class QARepository
    {
        private readonly string _connection;

        public QARepository(string connection)
        {
            _connection = connection;
        }

        public List<Question> GetAll()
        {
            using QandADataContext context = new QandADataContext(_connection);
            return context.Questions
                .Include(q => q.Answers)
                .Include(q => q.JoinTags)
                .ThenInclude(qt => qt.Tag)
                .ToList();
        }

        public Tag CheckIfExists(string name)
        {
            using QandADataContext context = new QandADataContext(_connection);
            return context.Tags.FirstOrDefault(t => t.Name == name);
        }

        public void AddQuestion(Question question, List<string> tags)
        {
            using QandADataContext context = new QandADataContext(_connection);
            context.Questions.Add(question);
            context.SaveChanges();
            int questionId = question.Id;

            foreach (var t in tags)
            {
                Tag check = CheckIfExists(t);
                
                if (check == null)
                {
                    check = new Tag { Name = t };
                    context.Tags.Add(check);
                    context.SaveChanges();
                }

                QTJoining qt = new QTJoining
                {
                    QuestionId = questionId,
                    TagId = check.Id
                };

                context.JoiningTable.Add(qt);
                context.SaveChanges();
            }
        }

        public Question GetById(int id)
        {
            using QandADataContext context = new QandADataContext(_connection);
            return context.Questions
                .Include(q => q.Answers)
                .Include(q => q.JoinTags)
                .ThenInclude(jt => jt.Tag)
                .FirstOrDefault(q => q.Id == id);
        }

        public void AddAnswer(Answer answer)
        {
            using QandADataContext context = new QandADataContext(_connection);
            context.Answers.Add(answer);
            context.SaveChanges();
        }

        public List<Tag> GetQuestionsByTag(int tagId)
        {
            using QandADataContext context = new QandADataContext(_connection);
            return context.Tags
                .Where(t => t.Id == tagId)
                .Include(t => t.JoinQuestions)
                .ThenInclude(jq => jq.Question)
                .ThenInclude(q => q.JoinTags)
                .ThenInclude(jt => jt.Tag)
                .ToList();
        }

        public List<Tag> GetAllTags()
        {
            using QandADataContext context = new QandADataContext(_connection);
            return context.Tags.ToList();
        }

        public List<Answer> GetAnswersById(int questionId)
        {
            using QandADataContext context = new QandADataContext(_connection);
            return context.Answers.Where(a => a.QuestionId == questionId).ToList();
        }
    }
}
