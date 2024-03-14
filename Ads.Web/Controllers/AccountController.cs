using Ads.Data;
using Ads.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ads.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _conStr = @"Data Source=.\sqlexpress; Initial Catalog=WebDevPractice2; Integrated Security=True;";

        public IActionResult Login()
        {
            return View(new LoginViewModel { Message = (string)TempData["Message"] });
        }

        [HttpPost]
        public IActionResult Login(string password, string email)
        {
            var userRepo = new UserRepository(_conStr);
            if (!userRepo.Login(email, password))
            {
                TempData["Message"] = "Invalid Login!";
                return RedirectToAction("Login");
            }

            Login(email);
            return RedirectToAction("NewAd", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("index", "home");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            var userRepo = new UserRepository(_conStr);
            userRepo.AddUser(user);
            Login(user.Email);
            return RedirectToAction("NewAd", "Home");
        }

        private void Login(string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "roles"))).Wait();

        }

    }
}
