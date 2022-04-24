using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QandA.Data;
using QandA.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QandA.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()

        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QandARepository(connectionString);



            HomeVeiwModel vm = new HomeVeiwModel();
            var questions = repo.GetAll();
            foreach (Question q in questions)
            {
                if (q.Text.Length > 300)
                {
                    q.Text = q.Text.Substring(0, 200) + ". . .";
                }
                foreach (Answer a in q.Answers)
                    if (a.Text.Length> 300)
                {
                    a.Text = a.Text.Substring(0, 200) + ". . .";
                }
            }

            vm.Questions = questions;


            return View(vm);
        }
        public IActionResult ViewQuestion(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QandARepository(connectionString);

            ViewQuestionModel vm = new();
            vm.Question = repo.GetQuestionById(id);
            if (User.Identity.IsAuthenticated)
            {
                vm.IsLoggedIn = true;
                User q = repo.GetUserByEmail(User.Identity.Name);
                vm.AlreadyLiked = repo.AlreadyLiked(id,q.Id);
            }
            return View(vm);
        }
[HttpPost]
        public void AddLike(Likes like)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            QandARepository repo = new(connectionString);
         
            string email = User.Identity.Name;
        
            User q = repo.GetUserByEmail(email);
            like.UserId = q.Id;
            repo.AddLike(like);
         
        }
        [Authorize]
        public IActionResult AskAQuestion()
        {
            
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AskAQuestion(Question question, List<string> tags)
        {
            question.DatePosted = DateTime.Now;
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QandARepository(connectionString);
            question.UserId = repo.GetByEmail(User.Identity.Name).Id;
            repo.AddQuestion(question, tags);
            //  return Redirect($"home/viewQuestion?id={question.Id}");
            return Redirect($"/home/viewQuestion?id={question.Id}");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer answer,int questionId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            QandARepository repo = new(connectionString);
            answer.QuestionId = questionId;
            answer.DatePosted = DateTime.Now;
            answer.UserId = repo.GetByEmail(User.Identity.Name).Id;
            repo.AddAnswer(answer);
            return Redirect($"/home/viewQuestion?id={answer.QuestionId}");
        }
        public IActionResult GetLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            QandARepository repo = new(connectionString);
           int likes= repo.GetLikesByQuestionId(id);
            return Json(likes);
        }
        public IActionResult GetQuestionsForTag(string tagname)

        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QandARepository(connectionString);



            HomeVeiwModel vm = new HomeVeiwModel();
            var questions = repo.GetQuestionsForTag(tagname);
            foreach (Question q in questions)
            {
                if (q.Text.Length > 300)
                {
                    q.Text = q.Text.Substring(0, 200) + ". . .";
                }
                foreach (Answer a in q.Answers)
                    if (a.Text.Length > 300)
                    {
                        a.Text = a.Text.Substring(0, 200) + ". . .";
                    }
            }

            vm.Questions = questions;


            return View(vm);
        }

    }
}
