using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;
using SSRD.Data;
using Microsoft.EntityFrameworkCore;

namespace SSRD.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ILogger<AuthorController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAuthors()
        {
            _logger.LogInformation("Retrieving authors.");

            var authors = await _context.Authors
                    .ToListAsync();

            return Ok(authors);            
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Author>>> GetAuthor([FromRoute]int id)
        {
            _logger.LogInformation("Retrieving authors.");

            var author = await _context.Authors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            
            if(author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> AddAuthor([FromBody] Author authorReq)
        {
            _logger.LogInformation("Adding author to database.");

            var authorDb = await _context.Authors
                .Where(x => x.Name == authorReq.Name &&
                            x.URL == authorReq.URL)
                .FirstOrDefaultAsync();

            if (authorDb != null)
            {
                _logger.LogError("User already exists.");
                return Forbid();
            }

            var author = new Author
            {
                Name = authorReq.Name,
                URL = authorReq.URL
            };

            if ((author.Name != null && author.Name != String.Empty) && (author.URL != null))
            {
                _context.Authors.Add(author);
            }

            int changes = await _context.SaveChangesAsync();
            if(changes <= 0)
            {
                _logger.LogError("Failed to add an author - database error.");
                return StatusCode(500);
            }
            return Ok(author);
        }
        [HttpPut]
        public async Task<ActionResult<Author>> UpdateAuthor([FromBody] Author authorReq)
        {
            _logger.LogInformation("Updating author {@obj}", new {
                authorId=authorReq.Id
            });

            var author = await _context.Authors
                .Where(a => a.Id == authorReq.Id)
                .FirstOrDefaultAsync();

            if (author == null)
            {
                return NotFound();
            }
            else
            {
                if(authorReq.Name != null && authorReq.Name != String.Empty)
                    author.Name = authorReq.Name;
                if (authorReq.URL != null && authorReq.URL != String.Empty)
                    author.URL = authorReq.URL;
            }
            _context.Authors.Update(author);

            int changes = await _context.SaveChangesAsync();
            if (changes <= 0)
            {
                _logger.LogError("Failed to update an author - database error.");
                return StatusCode(500);
            }
            return Ok(author);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Author>> Delete([FromRoute] int id)
        {
            _logger.LogInformation("Deleting author {@obj}", new {
                authorId = id
            });

            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            int changes = await _context.SaveChangesAsync();
            if (changes <= 0)
            {
                _logger.LogError("Failed to delete the author - database error.");
                return StatusCode(500);
            }
            return Ok(author);
        }
    }
}
