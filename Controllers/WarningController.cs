using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;
using SSRD.Data;
using Microsoft.EntityFrameworkCore;

namespace SSRD.Controllers
{
    
    public class WarningController : BaseController
    {
        private readonly DataContext _context;
        private readonly ILogger<WarningController> _logger;

        public WarningController(ILogger<WarningController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Warning>>> GetWarnings()
        {
            _logger.LogInformation("Retrieving all warnings.");

            List<Warning> warnings = await _context.Warnings
                    .Include(x => x.Author)
                    .ToListAsync();

            return Ok(warnings);
        }
        [HttpGet]
        [Route("{severity}")]
        public async Task<ActionResult<List<Warning>>> GetBySeverity([FromRoute] string severity)
        {
            _logger.LogInformation("Retrieving warnings with severity = {}.", severity);

            List<Warning> warnings = await _context.Warnings
                .Include(x => x.Author)
                .Where(w => w.Severity.ToLower() == severity.ToLower())
                .ToListAsync();
            
            return Ok(warnings);
        }
        [HttpPost]
        public async Task<ActionResult<Warning>> AddWarning([FromBody] Warning warningReq)
        {
            _logger.LogInformation("Adding warning to database.");

            var warning = new Warning
            {
                Region = warningReq.Region,
                Severity = warningReq.Severity,
                Onset = warningReq.Onset,
                AuthorId = warningReq.AuthorId
            };

            if ((warning.Region != null && warning.Region != String.Empty) &&
                (warning.Severity != null && warning.Severity != String.Empty))
            {
                _context.Warnings.Add(warning);
            }

            int changes = await _context.SaveChangesAsync();
            if (changes <= 0)
            {
                _logger.LogError("Failed to add a warning - database error.");
                return StatusCode(500);
            }
            return Ok(warning);
        }
        [HttpPut]
        public async Task<ActionResult<Warning>> UpdateWarning([FromBody] Warning warningReq)
        {
            _logger.LogInformation("Updating warning {@obj}.", new {
                warningId=warningReq.Id
            });

            Warning? warning = await _context.Warnings
                .Where(w => w.Id == warningReq.Id)
                .FirstOrDefaultAsync();

            if (warning == null)
            {
                _logger.LogError("Warning not found, {@obj}", new {
                    warningId = warningReq.Id
                });
                return NotFound();
            }

            if (warningReq.Region != null && warningReq.Region != String.Empty)
                warning.Region = warningReq.Region;
            if (warning.Severity != null && warning.Severity != String.Empty)
                warning.Severity = warningReq.Severity;
            if (warning.Onset != null)
                warning.Onset = warningReq.Onset;

            _context.Warnings.Update(warning);
            int changes = await _context.SaveChangesAsync();
            if (changes <= 0)
            {
                _logger.LogError("Failed to update a warning - database error.");
                return StatusCode(500);
            }

            return Ok(warning);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Warning>> Delete([FromRoute] int id)
        {
            _logger.LogInformation("Deleting warning {@obj}", new {
                warningId = id
            });

            var warning = await _context.Warnings
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync();

            if (warning == null)
            {
                _logger.LogError("Warning not found. {@obj}", new {
                    warningId = id
                });
                return NotFound();
            }

            _context.Warnings.Remove(warning);
            int changes = await _context.SaveChangesAsync();
            if (changes <= 0)
            {
                _logger.LogError("Failed to delete the warning - database error.");
                return StatusCode(500);
            }
            return Ok(warning);
        }
    }
}
