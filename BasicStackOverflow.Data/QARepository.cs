using Microsoft.EntityFrameworkCore;
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
            QandADataContext context = new QandADataContext(_connection);
            return context.Questions.Include(q => q.Answers).Include(q => q.JoinTags).ThenInclude(qt => qt.Tag).ToList();
        }

        public Tag CheckIfExists(string name)
        {
            QandADataContext context = new QandADataContext(_connection);
            return context.Tags.FirstOrDefault(t => t.Name == name);
        }

        public void AddQuestion(Question q, List<string> tags)
        {
            QandADataContext context = new QandADataContext(_connection);
            context.Questions.Add(q);
            context.SaveChanges();
            int questionId = q.Id;

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
                    Question = q,
                    Tag = check,
                    QuestionId = questionId,
                    TagId = check.Id
                };

                context.JoiningTable.Add(qt);
                context.SaveChanges();
            }
        }

        public Question GetById(int id)
        {
            QandADataContext context = new QandADataContext(_connection);
            return context.Questions.Include(q => q.Answers).Include(q => q.JoinTags).ThenInclude(jt => jt.Tag).FirstOrDefault(q => q.Id == id);
        }

        public void AddAnswer(Answer a)
        {
            QandADataContext context = new QandADataContext(_connection);
            context.Answers.Add(a);
            context.SaveChanges();
        }
    }
}
