using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;
using SSRD.Data;

namespace SSRD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ILogger<AuthorController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> Get()
        {
            try
            {
                var authors = _context.Authors.ToList();
                return Ok(authors);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            
        }
        [HttpPost]
        public async Task<ActionResult<Author>> AddAuthor(Author author)
        {
            var _author = new Author
            {
                Name = author.Name,
                URL = author.URL
            };

            if ((_author.Name != null && _author.Name != String.Empty) && (_author.URL != null))
            {
                try
                {
                    _context.Authors.Add(_author);
                    _context.SaveChanges();
                    return Ok(_author);
                }
                catch(Exception e)
                {
                    return StatusCode(500);
                }
            }
            else if(_author.Name == null || _author.Name == String.Empty)
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(500);
            }
        }
        [HttpPut]
        public async Task<ActionResult<Author>> UpdateAuthor(Author author)
        {
            var _author = _context.Authors.FirstOrDefault(a => a.Id == author.Id);
            if (_author == null)
            {
                return NotFound();
            }
            else
            {
                if(author.Name != null && author.Name != String.Empty)
                    _author.Name = author.Name;
                if (author.URL != null && author.URL != String.Empty)
                    _author.URL = author.URL;
                try
                {
                    _context.Authors.Update(_author);
                    _context.SaveChanges();
                    return Ok(_author);
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> Delete(int id)
        {
            var _author = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (_author == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _context.Authors.Remove(_author);
                    _context.SaveChanges();
                    return Ok(_author);
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
            }
        }
    }
}
