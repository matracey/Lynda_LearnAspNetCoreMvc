using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalifornia.api
{
    [Route("api/posts/{postKey}/comments")]
    public class CommentsController : Controller
    {
        private readonly BlogDataContext _db;

        public CommentsController(BlogDataContext db)
        {
            _db = db;
        }

        // GET: api/values
        [HttpGet]
        public IQueryable<Comment> Get(string postKey)
        {
            return _db.Comments.Where(x => x.Post.Key == postKey);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Comment> Get(long id)
        {
            var comment = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);
            return comment;
        }

        // POST api/values
        [HttpPost]
        public async Task<Comment> Post(string postKey, [FromBody]Comment comment)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(x => x.Key == postKey);

            if (post == null) return null;

            comment.Post = post;
            comment.Posted = DateTime.Now;
            comment.Author = User.Identity.Name;

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return comment;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(long id, [FromBody]Comment updated)
        {
            var comment = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return NotFound();

            comment.Body = updated.Body;

            await _db.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(long id)
        {
            var comment = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null) return;
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
        }
    }
}
