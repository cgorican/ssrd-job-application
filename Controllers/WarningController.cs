using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;
using SSRD.Data;

namespace SSRD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WarningController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<WarningController> _logger;

        public WarningController(ILogger<WarningController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Warning>>> Get()
        {
            try
            {
                var warnings = _context.Warnings.ToList();
                var authors = _context.Authors.ToList();
                return Ok(warnings);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        [HttpGet("{severity}")]
        public async Task<ActionResult<List<Warning>>> Get(string severity)
        {
            try
            {
                var warnings = _context.Warnings.ToList();
                var authors = _context.Authors.ToList();
                var severityWarnings = warnings.FindAll(w => w.Severity.ToLower() == severity.ToLower());
                return Ok(severityWarnings);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Warning>> AddWarning(Warning warning)
        {
            var _warning = new Warning
            {
                Region = warning.Region,
                Severity = warning.Severity,
                Onset = warning.Onset,
                AuthorId = warning.AuthorId
            };

            if ((_warning.Region != null && _warning.Region != String.Empty) &&
                (_warning.Severity != null && _warning.Severity != String.Empty))
            {
                try
                {
                    _context.Warnings.Add(_warning);
                    _context.SaveChanges();
                    return Ok(_warning);
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
            }
            else if ((_warning.Region == null || _warning.Region == String.Empty) ||
                     (_warning.Severity == null || _warning.Severity == String.Empty))
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(500);
            }
        }
        [HttpPut]
        public async Task<ActionResult<Warning>> UpdateWarning(Warning warning)
        {
            var _warning = _context.Warnings.FirstOrDefault(w => w.Id == warning.Id);
            if (_warning == null)
            {
                return NotFound();
            }
            else
            {
                if (warning.Region != null && warning.Region != String.Empty)
                    _warning.Region = warning.Region;
                if (_warning.Severity != null && _warning.Severity != String.Empty)
                    _warning.Severity = warning.Severity;
                if (_warning.Onset != null)
                    _warning.Onset = warning.Onset;
                try
                {
                    _context.Warnings.Update(_warning);
                    _context.SaveChanges();
                    return Ok(_warning);
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Warning>> Delete(int id)
        {
            var _warning = _context.Warnings.FirstOrDefault(w => w.Id == id);
            if (_warning == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _context.Warnings.Remove(_warning);
                    _context.SaveChanges();
                    return Ok(_warning);
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
            }
        }
    }
}
