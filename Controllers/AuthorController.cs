using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;

namespace SSRD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Author>>> Get()
        {
            var warnings = new List<Warning>()
            {
                new Warning {
                    Id = 1,
                    Severity = "Severe",
                    Onset = DateTime.Parse("2022-07-29T18:55:09+00:00"),
                    AuthorId = null
                }
            };
            
            return Ok(warnings);
        }
        [HttpPost]
        public async Task<ActionResult<List<Warning>>> AddAuthor(Author author)
        {
            var newAuthor = 0; //new Warning();
            // INSERT INTO Warning () VALUES ();
            // INSERT INTO Warning VALUES ();

            return Ok(newAuthor);
        }
        [HttpPut]
        public async Task<ActionResult<List<Warning>>> UpdateAuthor(Author author)
        {
            // UPDATE Warning SET col1 = val1, ... WHERE id = id;
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Warning>>> Delete(int id)
        {
            // DELETE FROM Warning WHERE id = id;
            return Ok();
        }
    }
}
