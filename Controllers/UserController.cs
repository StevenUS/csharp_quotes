using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using quoteRedux.Models;
using quoteRedux.Factory; //Need to include reference to new Factory Namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace quoteRedux.Controllers
{
    public class UserController : Controller
    {
        private readonly UserFactory userFactory;
        public UserController(UserFactory user)
        {
            //This is establish the initial DB connection for us.
            userFactory = user;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {    
            ViewBag.showErrors = false;
            return View("Index");
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User newUser)
        {
            if(userFactory.FindByEmail(newUser.email) != null)
            {
                ViewBag.emailInUse = "email in use";
                ViewBag.showErrors = false;
            }
            else
            {
                if(!ModelState.IsValid)
                {
                    ViewBag.showErrors = true;
                    ViewBag.errors = ModelState.Values;
                }
                else
                {
                    ViewBag.showErrors = false;
                    ViewBag.showSuccess = "Registration successful";

                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.password = Hasher.HashPassword(newUser, newUser.password);

                    userFactory.Add(newUser);
                }
            }
            return View("Index");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            var user = userFactory.FindByEmail(Email);
            if(user != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.password, Password))
                {
                    HttpContext.Session.SetInt32("userID", user.id);
                    ViewBag.userID = HttpContext.Session.GetInt32("userID");
                    //checks all controllers routes (RedirectToRoute)
                    return RedirectToAction("GetQuote", "Quote");
                }
            }
            ViewBag.showErrors = false;
            ViewBag.loginError = "email or password is incorrect";
            return View("Index");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            //checks within the current controller only (RedirectToAction)
            return RedirectToAction("Index");
        }
    }
}