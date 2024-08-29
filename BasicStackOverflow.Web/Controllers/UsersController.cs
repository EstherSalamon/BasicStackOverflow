using BasicStackOverflow.Data;
using BasicStackOverflow.Web.Views;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicStackOverflow.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connection;

        public UsersController(IConfiguration config)
        {
            _connection = config.GetConnectionString("ConStr");
        }

        [HttpPost("signup")]
        public void SignUp(SignUpVM vm)
        {
            UserRepository repo = new UserRepository(_connection);
            repo.AddUser(vm.User, vm.Password);
        }

        [HttpPost("login")]
        public User LogIn(LogInVM vm)
        {
            UserRepository repo = new UserRepository(_connection);
            User user = repo.LogIn(vm.Email, vm.Password);

            if (user == null)
            {
                return null;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "cookies", ClaimTypes.Email, "role"))).Wait();

            return user;
        }

        [HttpGet("currentuser")]
        public User GetCurrentUser()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return null;
            }

            UserRepository repo = new UserRepository(_connection);
            return repo.GetByEmail(User.Identity.Name);
        }

        [HttpPost("logout")]
        public void LogOut()
        {
            HttpContext.SignOutAsync().Wait();
        }

        [HttpGet("emailexists")]
        public bool EmailExists(string email)
        {
            UserRepository repo = new UserRepository(_connection);
            return repo.EmailExists(email);
        }
    }

}

