using System;
using System.Collections.Generic;
using quoteRedux.Models;
using Microsoft.AspNetCore.Mvc;
using quoteRedux.Factory; //Need to include reference to new Factory Namespace
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace quoteRedux.Controllers
{
    public class QuoteController : Controller
    {
        private readonly QuoteFactory quoteFactory;
        public QuoteController(QuoteFactory quote)
        {
            //Instantiate a QuoteFactory object that is immutable (READONLY)
            //This is establish the initial DB connection for us.
            quoteFactory = quote;
        }
        // GET: /Home/
        [HttpGet]
        [Route("quote")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
        [HttpPost]
        [Route("add_quote")]
        public IActionResult AddQuote(Quote NewQuote)
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction("Index");
            }
            int userId = (int)HttpContext.Session.GetInt32("userID");
            quoteFactory.Add(NewQuote, userId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("quotes")]
        public IActionResult GetQuote()
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction("Index");
            }
            //We can call upon the methods of the userFactory directly now.
             int userId = (int)HttpContext.Session.GetInt32("userID");
             ViewBag.UserID = userId;
            ViewBag.Quotes = quoteFactory.FindAll();
            return View("Quotes");
        }
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult DeleteQuote(int id)
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction("Index");
            }
            quoteFactory.DeleteByID(id);
            return RedirectToAction("GetQuote");
        }
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult ShowEditQuote(int id)
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.prevText =  quoteFactory.FindByID(id);
            return View("Edit");
        }
        [HttpPost]
        [Route("edit_quote")]
        public IActionResult EditQuote(string text, int id)
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction("Index");
            }
            quoteFactory.Edit(id, text);
            return RedirectToAction("GetQuote");
        }
    }
}
