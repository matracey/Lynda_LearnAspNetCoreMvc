using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ExploreCalifornia.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            var posts = new[]
            {
                new Post
                {
                    Title = "My blog post",
                    Posted = DateTime.Now,
                    Author = "Martin Tracey",
                    Body = "This is a great blog post, don't you think?"
                },
                    new Post
                {
                    Title = "My second blog post",
                    Posted = DateTime.Now,
                    Author = "Martin Tracey",
                    Body = "This is a ANOTHER great blog post, don't you think?"
                }
            };

            return View(posts);
        }

        [Route("{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key)
        {
            var post = new Post
            {
                Title = "My blog post",
                Posted = DateTime.Now,
                Author = "Martin Tracey",
                Body = "This is a great blog post, don't you think?"
            };

            return View(post);
        }
    }
}