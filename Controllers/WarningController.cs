using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;

namespace SSRD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WarningController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Warning>>> Get()
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
        [HttpGet("{severity}")]
        public async Task<ActionResult<List<Warning>>> Get(string severity)
        {
            var warnings = new List<Warning>();
            var severityWarnings = warnings.Find(w => w.Severity == severity);
            // SELECT * FROM Warning WHERE severity = "severe";

            return Ok(severityWarnings);
        }
        [HttpPost]
        public async Task<ActionResult<List<Warning>>> AddWarning(Warning warning)
        {
            var newWarning = 0; //new Warning();
            // INSERT INTO Warning () VALUES ();
            // INSERT INTO Warning VALUES ();

            return Ok(newWarning);
        }
        [HttpPut]
        public async Task<ActionResult<List<Warning>>> UpdateWarning(Warning warning)
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
