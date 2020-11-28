using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using AngularProjectAPI.Models.LikeModels;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly NewsContext _context;

        public LikeController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Like
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
        {
            return await _context.Likes.ToListAsync();
        }

        [HttpGet("likes/articles/{titelName}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesByArticleTitle(string titelName)
        {
            return await _context.Likes.Include(u=>u.User).Include(a=>a.Article).Where(a=>a.Article.Title==titelName).ToListAsync();
        }


        [HttpGet("likes/articles")]
        public async Task<ActionResult<IEnumerable<Like>>> getArticlesPublishedByLikes()
        {
            return await _context.Likes.Include(u => u.User).Include(a => a.Article)
                                         .Where(a => a.Article.ArticleStatus.Name == "Published").OrderBy(n=>n.Number).ToListAsync();

        }



        // GET: api/Like/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);

            if (like == null)
            {
                return NotFound();
            }

            return like;
        }





        // PUT: api/Like/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLike(int id, Like like)
        {
            if (id != like.LikeID)
            {
                return BadRequest();
            }

            _context.Entry(like).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<LikePost>> PostLike(LikePost like)
        {
            Like newLike = new Like();
            newLike.Number = like.Number;
            newLike.UserID = like.UserID;
            newLike.ArticleID = like.ArticleID;

            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = like.LikeID }, like);
        }



        // GET: api/Like/5
        [HttpDelete("user/{id}")]
        public async Task<ActionResult<Like>> deleteLikeByUser(int id)
        {
            var like = await _context.Likes.Include(u => u.User).Where(u => u.User.UserID == id).FirstAsync();

            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return like;
        }




        // DELETE: api/Like/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Like>> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return like;
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.LikeID == id);
        }
    }
}
