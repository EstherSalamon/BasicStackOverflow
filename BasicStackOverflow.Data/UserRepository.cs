using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicStackOverflow.Data
{
    public class UserRepository
    {
        private readonly string _connection;

        public UserRepository(string connection)
        {
            _connection = connection;
        }

        public void AddUser(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            QandADataContext context = new QandADataContext(_connection);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User LogIn(string email, string password)
        {
            User user = GetByEmail(email);

            if(user == null)
            {
                return null;
            }

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if(!verifyPassword)
            {
                return null;
            }

            return user;
        }

        public bool EmailExists(string email)
        {
            QandADataContext context = new QandADataContext(_connection);
            return context.Users.Any(u => u.Email == email);
        }

        public User GetByEmail(string email)
        {
            QandADataContext context = new QandADataContext(_connection);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            QandADataContext context = new QandADataContext(_connection);
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
