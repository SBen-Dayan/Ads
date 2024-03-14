using Ads.Data;
using Ads.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ads.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _conStr = @"Data Source=.\sqlexpress; Initial Catalog=WebDevPractice2; Integrated Security=True;";

        public IActionResult Index()
        {
            var adRepo = new AdRepository(_conStr);
            return View(new AdsViewModel
            {
                Ads = adRepo.GetAds(),
                UserId = User.Identity.Name != null ? GetUserId() :0
            });
        }

        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(string phoneNumber, string details)
        {
            var adRepo = new AdRepository(_conStr);
            adRepo.Insert(new Ad
            {
                PhoneNumber = phoneNumber,
                Details = details,
                UserId = GetUserId(),
                Date = DateTime.Now
            });
            return RedirectToAction("index");
        }

        [Authorize]
        public IActionResult Account()
        {
            var adRepo = new AdRepository(_conStr);
            var userId = GetUserId();
            return View(new AccountViewModel { Ads = adRepo.GetAds(userId) });
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            var adRepo = new AdRepository(_conStr);
            adRepo.Delete(id);
            return RedirectToAction("index");
        }

        private int GetUserId()
        {
            var userRepo = new UserRepository(_conStr);
            return userRepo.GetId(User.Identity.Name);
        }
    }
}