using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using AngularProjectAPI.Models.ArticleModels;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly NewsContext _context;

        public ArticleController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Article
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.Include(t=>t.Tag).Include(u=>u.User).Include(a=>a.ArticleStatus).ToListAsync();
        }

        [HttpGet("review/articles")]
        public async Task<ActionResult<IEnumerable<Article>>> getArticlesToReview()
        {
            return await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus)
                                         .Where(a => a.ArticleStatus.Name == "To review").ToListAsync();

        }

        [HttpGet("published/articles")]
        public async Task<ActionResult<IEnumerable<Article>>> getArticlesPublished()
        {
            return await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus)
                                         .Where(a => a.ArticleStatus.Name == "Published").ToListAsync();

        }

        [HttpGet("published/likes/articles")]
        public async Task<ActionResult<IEnumerable<Article>>> getArticlesPublishedByLikes()
        {
            return await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus).Include(l=>l.Likes.Count())
                                         .Where(a => a.ArticleStatus.Name == "Published").OrderBy(l=>l.Likes).ToListAsync();

        }

        [HttpGet("published/articles/{tagName}")]
        public async Task<ActionResult<IEnumerable<Article>>> getArticlesPublishedByTag(string tagName)
        {
            return await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus)
                                         .Where(a => a.ArticleStatus.Name == "Published").Where(t=>t.Tag.Name == tagName).ToListAsync();

        }

        [HttpGet("title/{titleName}")]
        public async Task<ActionResult<Article>> getArticleByTitle(string titleName)
        {
            var article = await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus)
                                         .Where(a => a.Title == titleName).FirstAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpGet("review/title/{titleName}")]
        public async Task<ActionResult<Article>> getArticleByTitleToReview(string titleName)
        {
            var article = await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus)
                                         .Where(a => a.Title == titleName).Where(a=>a.ArticleStatus.Name=="To review").FirstAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpGet("published/title/{titleName}")]
        public async Task<ActionResult<Article>> getArticleByTitlePublished(string titleName)
        {
            var article = await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus)
                                         .Where(a => a.Title == titleName).Where(a => a.ArticleStatus.Name == "Published").FirstAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }


        // GET: api/Article/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Article/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        // POST: api/Article
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ArticlePost>> PostArticle(ArticlePost article)
        {
            Article newArticle = new Article();
            newArticle.Title = article.Title;
            newArticle.SubTitle = article.SubTitle;
            newArticle.ShortSummary = article.ShortSummary;
            newArticle.Body = article.Body;
            newArticle.Image = article.Image;
            newArticle.TagID = article.TagID;
            newArticle.UserID = article.UserID;
            newArticle.ArticleStatusID = article.ArticleStatusID;

            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.ArticleID }, article);
        }

        // DELETE: api/Article/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesByUserID(int id)
        {

            var articlesUser = await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(a => a.ArticleStatus).Where(u=>u.User.UserID==id).ToListAsync();

            if (articlesUser == null)
            {
                return NotFound();
            }

            return articlesUser;
        }


        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
