using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QandA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QandA.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
           
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QandARepository(connectionString);
           repo.Add(user, password);
            return Redirect("/account/login");
        }
        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Message = TempData["Error"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QandARepository(connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "Invalid login!";
                return Redirect("/account/login");
            }

           
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
